using System.Net;
using System.Net.Http.Json;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Tests.WebApplicationFactories;
using FluentAssertions;
using Moq;

namespace DeliveryProject.Tests.ControllersTests;

public class DeliveryControllerTests : IClassFixture<OrderAndDeliveryWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly OrderAndDeliveryWebApplicationFactory _factory;

    public DeliveryControllerTests(OrderAndDeliveryWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddInvoice_ShouldReturnOk_WithMessage()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _factory.DeliveryServiceMock
            .Setup(x => x.AddInvoice(orderId))
            .Returns(Task.CompletedTask);

        _factory.OrderServiceMock
            .Setup(x => x.UpdateOrderStatus(orderId, It.IsAny<OrderStatus>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.PostAsync($"/api/Delivery/Invoice/Add?orderId={orderId}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json.Should().ContainKey("message");
        json!["message"].Should().Be(string.Format(InfoMessages.AddedInvoice, orderId));
    }

    [Fact]
    public async Task AddInvoice_ShouldReturnBadRequest_WhenOrderIsNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _factory.OrderServiceMock
            .Setup(x => x.UpdateOrderStatus(orderId, OrderStatus.Active))
            .ThrowsAsync(new BussinessArgumentException(ErrorMessages.OrderNotFound));

        // Act
        var response = await _client.PostAsync($"/api/Delivery/Invoice/Add?orderId={orderId}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}