using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Dto;
using FluentAssertions;
using FluentValidation;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Bussiness.Services;
using DeliveryProject.Middleware;
using DeliveryProject.Tests.Assertions;
using DeliveryProject.Tests.Mocks;
using Newtonsoft.Json;
using DeliveryProject.DataAccess.Interfaces;

public class OrderControllerTests : BaseControllerTests, IClassFixture<WebApplicationFactory<Program>>
{
    public OrderControllerTests(WebApplicationFactory<Program> factory)
        : base(
            InitializeClient(factory, out var orderServiceMock, out var orderRepositoryMock),
            orderServiceMock,
            orderRepositoryMock) 
    {
    }

    private static HttpClient InitializeClient(WebApplicationFactory<Program> factory, out Mock<IOrderService> orderServiceMock
        , out Mock<IOrderRepository> orderRepositoryMock)
    {
        var localOrderServiceMock = OrderServiceMock.Create();
        var localOrderRepositoryMock = OrderRepositoryMock.Create();
        orderServiceMock = localOrderServiceMock;
        orderRepositoryMock = localOrderRepositoryMock;

        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IOrderService>(_ => localOrderServiceMock.Object);
                services.AddScoped<IOrderRepository>(_ => localOrderRepositoryMock.Object);
            });
        }).CreateClient();

        return client;
    }


    [Fact]
    public async Task FilterOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        var regionName = "District 1";

        // Act
        var response = await GetAsync($"/Orders/Filter?regionName={regionName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task FilterOrders_ShouldReturnBadRequest_WhenNoOrdersExist()
    {
        //Arrange
        OrderServiceMock.SetupBussinessArgumentException(_orderServiceMock, "The region with the name {regionName} was not found.");

        // Act
        var response = await GetAsync($"/Orders/Filter?regionName=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        OrderServiceMock.SetupGetAllOrders(_orderServiceMock, new List<Order>());

        // Act
        var response = await GetAsync("/Orders/GetAll");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOk_WhenNoOrdersExist()
    {
        // Arrange
        OrderServiceMock.SetupGetAllOrdersWithNull(_orderRepositoryMock);

        // Act
        var response = await GetAsync("/Orders/GetAll");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<Order>>(responseContent);
        orders.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnSortedOrders_WhenSortedByWeightAscending()
    {
        // Arrange
        _orderServiceMock
            .Setup(service => service.GetAllOrders(OrderSortField.Weight, false))
            .ReturnsAsync(new List<Order>
            {
            new Order { Weight = 1 },
            new Order { Weight = 2 },
            new Order { Weight = 3 }
            });

        // Act
        var response = await GetAsync("Orders/GetAll?sortBy=Weight&descending=false");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnBadRequest_WhenSortFieldIsInvalid()
    {
        // Act
        var response = await GetAsync("Orders/GetAll?sortBy=InvalidField&descending=false");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddOrder_ShouldIncludeStackTrace_InResponseForValidationException()
    {
        // Arrange
        var request = new AddOrderRequest
        {
            RegionId = 0,
            Weight = -5.5,
            DeliveryTime = DateTime.Now,
            SupplierId = 0
        };

        var jsonContent = JsonContent.Create(request);

        OrderServiceMock.SetupValidationException(_orderServiceMock, "Validation failed.");

        // Act
        var response = await _client.PostAsync("/api/order/Order/Add", jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        AssertResponseDetails(responseContent, "Validation failed.", nameof(ValidationMiddleware));
    }


    [Fact]
    public async Task FilterOrders_ShouldIncludeStackTrace_InResponseForBussinessArgumentException()
    {
        OrderServiceMock.SetupBussinessArgumentException(_orderServiceMock, "The RegionName field must not be empty.");


        // Act
        var response = await GetAsync($"Orders/Filter?regionName=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        AssertResponseDetails(responseContent, "The RegionName field must not be empty.", nameof(OrderService.FilterOrders));
    }
}