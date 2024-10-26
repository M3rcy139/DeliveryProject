using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Controllers;
using DeliveryProject.Persistence.Interfaces;
using DeliveryProject.Core.Models;
using DeliveryProject.Application.Contracts;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

public class OrderControllerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ILogger<OrderController>> _loggerMock;
    private readonly Mock<IValidator<AddOrderRequest>> _orderValidatorMock;
    private readonly OrderController _orderController;

    public OrderControllerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _loggerMock = new Mock<ILogger<OrderController>>();
        _orderValidatorMock = new Mock<IValidator<AddOrderRequest>>();

        _orderController = new OrderController(_loggerMock.Object, _orderRepositoryMock.Object, _orderValidatorMock.Object);
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

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("AreaId", "AreaId должен быть больше нуля."),
            new ValidationFailure("Weight", "Вес заказа должен быть положительным."),
            new ValidationFailure("DeliveryTime", "Время доставки не может быть в прошлом.")
        });
        _orderValidatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _orderController.AddOrder(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task FilterOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        var areaId = 1;
        var firstOrderTime = DateTime.Now;
        _orderRepositoryMock.Setup(repo => repo.GetFirstOrderTime(areaId)).ReturnsAsync(firstOrderTime);
        _orderRepositoryMock.Setup(repo => repo.GetOrdersWithinTimeRange(areaId, firstOrderTime, firstOrderTime.AddMinutes(30)))
            .ReturnsAsync(new List<Order>());

        // Act
        var result = await _orderController.FilterOrders(areaId);

        // Assert
        result.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task FilterOrders_ShouldReturnBadRequest_WhenNoOrdersExist()
    {
        // Arrange
        var areaId = 1;
        _orderRepositoryMock.Setup(repo => repo.GetFirstOrderTime(areaId)).ThrowsAsync(new ArgumentException("Заказов в данном районе не найдено"));

        // Act
        var result = await _orderController.FilterOrders(areaId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        _orderRepositoryMock.Setup(repo => repo.GetAllOrders()).ReturnsAsync(new List<Order>());

        // Act
        var result = await _orderController.GetAllOrders();

        // Assert
        result.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnBadRequest_WhenNoOrdersExist()
    {
        // Arrange
        _orderRepositoryMock.Setup(repo => repo.GetAllOrders()).ThrowsAsync(new ArgumentException("Заказы не найдены"));

        // Act
        var result = await _orderController.GetAllOrders();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
