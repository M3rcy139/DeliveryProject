using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Models;
using DeliveryProject.API.Dto;
using FluentAssertions;
using FluentValidation;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Bussiness.Services;
using DeliveryProject.API.Extensions;
using DeliveryProject.API.Middleware;

public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly HttpClient _client;

    public OrderControllerTests(WebApplicationFactory<Program> factory)
    {
        _orderServiceMock = new Mock<IOrderService>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IOrderService>(_ => _orderServiceMock.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task AddOrder_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new AddOrderRequest
        {
            RegionId = 0,
            Weight = -5.5,
            DeliveryTime = DateTime.Now,
            SupplierId = 1
        };

        var jsonContent = JsonContent.Create(request);

        _orderServiceMock
            .Setup(service => service.AddOrder(It.IsAny<Order>(), request.SupplierId))
            .ThrowsAsync(new ValidationException("Validation error."));

        // Act
        var response = await _client.PostAsync($"/api/order/Order/Add", jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task FilterOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        var regionName = "District 1";

        _orderServiceMock.Setup(service => service.FilterOrders(regionName))
            .ReturnsAsync(new List<Order>());

        // Act
        var response = await _client.GetAsync($"/api/order/Orders/Filter?regionName={regionName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task FilterOrders_ShouldReturnBadRequest_WhenNoOrdersExist()
    {
        //Arrange
        string? regionName = null;

        _orderServiceMock.Setup(service => service.FilterOrders(regionName))
            .ThrowsAsync(new BussinessArgumentException("The region with the name {regionName} was not found."));

        // Act
        var response = await _client.GetAsync($"/api/order/Orders/Filter?regionName={regionName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        _orderServiceMock
            .Setup(service => service.GetAllOrders(null, false))
            .ReturnsAsync(new List<Order> { new Order() });

        // Act
        var response = await _client.GetAsync("/api/order/Orders/GetAll");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnBadRequest_WhenNoOrdersExist()
    {
        // Arrange
        _orderServiceMock
            .Setup(service => service.GetAllOrders(null, false))
            .ThrowsAsync(new BussinessArgumentException("No orders found.", "NO_ORDERS_FOUND"));

        // Act
        var response = await _client.GetAsync($"/api/order/Orders/GetAll");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnSortedOrders_WhenSortedByWeightAscending()
    {
        // Arrange
        _orderServiceMock
            .Setup(service => service.GetAllOrders(SortField.Weight, false))
            .ReturnsAsync(new List<Order>
            {
            new Order { Weight = 1 },
            new Order { Weight = 2 },
            new Order { Weight = 3 }
            });

        // Act
        var response = await _client.GetAsync("/api/order/Orders/GetAll?sortBy=Weight&descending=false");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnBadRequest_WhenSortFieldIsInvalid()
    {
        // Act
        var response = await _client.GetAsync("/api/order/Orders/GetAll?sortBy=InvalidField&descending=false");

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
            SupplierId = 1
        };

        var jsonContent = JsonContent.Create(request);

        _orderServiceMock
            .Setup(service => service.AddOrder(It.IsAny<Order>(), request.SupplierId))
            .ThrowsAsync(new ValidationException("Validation error."));

        // Act
        var response = await _client.PostAsync($"/api/order/Order/Add", jsonContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        responseContent.Detail.Should().NotBeNullOrEmpty();
        responseContent.Detail.Should().Contain("Validation error.");
        responseContent.Detail.Should().Contain(nameof(ValidationMiddleware));
    }


    [Fact]
    public async Task FilterOrders_ShouldIncludeStackTrace_InResponseForBussinessArgumentException()
    {
        // Arrange
        string? regionName = null;

        _orderServiceMock
            .Setup(service => service.FilterOrders(regionName))
            .ThrowsAsync(new BussinessArgumentException("The RegionName field must not be empty."));

        // Act
        var response = await _client.GetAsync($"/api/order/Orders/Filter?regionName={regionName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        // Проверяем, что Detail содержит стек вызовов и сообщение об ошибке
        responseContent.Detail.Should().NotBeNullOrEmpty();
        responseContent.Detail.Should().Contain("The RegionName field must not be empty.");
        responseContent.Detail.Should().Contain(nameof(OrderService.FilterOrders));
    }

    [Fact]
    public async Task GetAllOrders_ShouldIncludeStackTraceAndErrorCode_InResponse()
    {
        // Arrange
        _orderServiceMock
            .Setup(service => service.GetAllOrders(null, false))
            .ThrowsAsync(new BussinessArgumentException("No orders found."));

        // Act
        var response = await _client.GetAsync("/api/order/Orders/GetAll");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();

        // Проверяем сообщение об ошибке и стек вызовов
        responseContent.Detail.Should().NotBeNullOrEmpty();
        responseContent.Detail.Should().Contain("No orders found.");
        responseContent.Detail.Should().Contain(nameof(OrderService.GetAllOrders));
    }

}