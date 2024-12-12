using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Controllers;
using DeliveryProject.Core.Interfaces.Services;
using DeliveryProject.Core.Models;
using DeliveryProject.Application.Dto;
using FluentAssertions;
using FluentValidation;
using System.Net;

public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrderController _orderController;
    private readonly Mock<IValidator<AddOrderRequest>> _orderValidatorMock;
    private readonly HttpClient _client;

    public OrderControllerTests(WebApplicationFactory<Program> factory)
    {
        _orderServiceMock = new Mock<IOrderService>();

        _orderController = new OrderController(_orderServiceMock.Object);
        _orderValidatorMock = new Mock<IValidator<AddOrderRequest>>();

        _client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task AddOrder_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new AddOrderRequest
        (
            0, 
            -5.5, 
            DateTime.Now
        );

        // Act
        var response = await _client.GetAsync($"/api/order/add-order?request={request}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task FilterOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        var areaId = 1;

        // Act
        var result = await _orderController.FilterOrders(areaId);

        // Assert
        result.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task FilterOrders_ShouldReturnBadRequest_WhenNoOrdersExist()
    {
        //Arrange
        var areaId = 100;

        // Act
        var response = await _client.GetAsync($"/api/order/filter-orders?areaId={areaId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        _orderServiceMock.Setup(repo => repo.GetAllOrders()).ReturnsAsync(new List<Order>());

        // Act
        var result = await _orderController.GetAllOrders();

        // Assert
        result.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnBadRequest_WhenNoOrdersExist()
    {
        // Arrange
        _orderServiceMock.Setup(repo => repo.GetAllOrders()).ThrowsAsync(new ArgumentException("Заказы не найдены"));

        // Act
        var response = await _client.GetAsync($"/api/order/gett-all-orders");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
