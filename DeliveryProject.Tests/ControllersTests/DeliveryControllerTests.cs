using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Controllers;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Exceptions;

public class DeliveryControllerTests
{
    private readonly Mock<IDeliveryService> _deliveryServiceMock = new();
    private readonly Mock<IOrderService> _orderServiceMock = new();
    private readonly DeliveryController _controller;

    public DeliveryControllerTests()
    {
        _controller = new DeliveryController(_deliveryServiceMock.Object, _orderServiceMock.Object);
    }

    [Fact]
    public async Task AddInvoice_Should_Return_Success_Message()
    {
        var orderId = Guid.NewGuid();

        var result = await _controller.AddInvoice(orderId) as OkObjectResult;

        result.Should().NotBeNull();
        var message = result!.Value!.GetType().GetProperty("message")?.GetValue(result.Value, null)?.ToString();
        message.Should().Be(string.Format(InfoMessages.AddedInvoice, orderId));
        
        _deliveryServiceMock.Verify(x => x.AddInvoice(orderId), Times.Once);
        _orderServiceMock.Verify(x => x.UpdateOrderStatus(orderId, OrderStatus.Active), Times.Once);
    }

    [Fact]
    public async Task AddInvoice_Should_Return_Not_Order_Found()
    {
        var orderId = Guid.NewGuid();
        _orderServiceMock.Setup(x => x.UpdateOrderStatus(orderId, OrderStatus.Active))
            .ThrowsAsync(new BussinessArgumentException(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound));

        Func<Task> act = async () => await _controller.AddInvoice(orderId);

        await act.Should().ThrowAsync<Exception>().WithMessage(ErrorMessages.OrderNotFound);
    }
}