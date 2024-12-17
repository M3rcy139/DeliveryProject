using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using DeliveryProject.Bussiness.Contract.Interfaces.Services;
using DeliveryProject.Core.Models;
using DeliveryProject.API.Dto;
using FluentAssertions;
using FluentValidation;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using DeliveryProject.Core.Exceptions;

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
            regionId = 0,
            Weight = -5.5,
            DeliveryTime = DateTime.Now
        };

        var jsonContent = JsonContent.Create(request);

        _orderServiceMock
            .Setup(service => service.AddOrder(It.IsAny<Order>()))
            .ThrowsAsync(new ValidationException("Ошибка валидации"));

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
        var regionName = "";

        _orderServiceMock.Setup(service => service.FilterOrders(regionName))
            .ThrowsAsync(new BussinessArgumentException("Заказы не найдены"));

        // Act
        var response = await _client.GetAsync($"/api/order/Orders/Filter?regionName={regionName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        _orderServiceMock.Setup(repo => repo.GetAllOrders()).ReturnsAsync(new List<Order>());

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
            .Setup(service => service.GetAllOrders())
            .ThrowsAsync(new BussinessArgumentException("Заказы не найдены"));

        // Act
        var response = await _client.GetAsync($"/api/order/Orders/GetAll");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
