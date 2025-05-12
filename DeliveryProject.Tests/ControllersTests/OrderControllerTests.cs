using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Controllers;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.Tests.ControllersTests;

public class OrderControllerTests
{
    private readonly Mock<IOrderService> _orderServiceMock = new();
    private readonly Mock<IDeliveryService> _deliveryServiceMock = new();
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _controller = new OrderController(_orderServiceMock.Object, _deliveryServiceMock.Object);
    }

    [Fact]
    public async Task CreateOrder_Should_Return_Message_With_Generated_Id()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<ProductDto>()
        };

        // Act
        var result = await _controller.CreateOrder(dto) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var message = result.Value!.GetType().GetProperty("message")?.GetValue(result.Value, null)?.ToString();
        message.Should().StartWith("Added an order with id: ");
    }
    
    [Fact]
    public async Task GetOrderById_Should_Return_Order()
    {
        //Arrange
        var orderId = Guid.NewGuid();
        var expected = new Order { Id = orderId };
        _orderServiceMock.Setup(s => s.GetOrderById(orderId)).ReturnsAsync(expected);

        //Act
        var result = await _controller.GetOrderById(orderId) as OkObjectResult;

        
        //Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task GetOrdersByRegionId_Should_Respect_Sorting_Params()
    {
        //Arrange
        var regionId = 1;
        var expected = new List<Order> { new() { Id = Guid.NewGuid() } };
        _orderServiceMock
            .Setup(s => s.GetOrdersByRegionId(regionId, OrderSortField.CreatedTime, true))
            .ReturnsAsync(expected);

        //Act
        var result = await _controller.GetOrdersByRegionId(regionId, OrderSortField.CreatedTime, true) as OkObjectResult;

        
        //Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateOrder_Should_Return_Success_Message()
    {
        //Arrange
        var dto = new UpdateOrderDto
        {
            Id = Guid.NewGuid(),
            Products = new List<ProductDto>()
        };

        
        //Act
        var result = await _controller.UpdateOrder(dto) as OkObjectResult;

        //Assert
        result.Should().NotBeNull();
        var message = result!.Value!.GetType().GetProperty("message")?.GetValue(result.Value, null)?.ToString();
        message.Should().Be(string.Format(InfoMessages.UpdatedOrder, dto.Id));
    }

    [Fact]
    public async Task RemoveOrder_Should_Return_Removed_Message()
    {
        //Arrange
        var orderId = Guid.NewGuid();

        //Act
        var result = await _controller.RemoveOrder(orderId) as OkObjectResult;

        //Assert
        result.Should().NotBeNull();
        var message = result!.Value!.GetType().GetProperty("message")?.GetValue(result.Value, null)?.ToString();
        message.Should().Be(InfoMessages.RemovedOrder);
    }
}
