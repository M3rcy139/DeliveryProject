using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Core.Models;
using DeliveryProject.Tests.WebApplicationFactories;
using FluentAssertions;
using Moq;

namespace DeliveryProject.Tests.ControllersTests;

public class OrderControllerTests : IClassFixture<OrderAndDeliveryWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly OrderAndDeliveryWebApplicationFactory _factory;

    public OrderControllerTests(OrderAndDeliveryWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnOk_WithMessage()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<ProductDto> { new ProductDto { ProductId = Guid.NewGuid(), Quantity = 1}}
        };

        _factory.OrderServiceMock
            .Setup(x => x.CreateOrder(It.IsAny<Order>(), It.IsAny<List<ProductDto>>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.PostAsJsonAsync("/api/Order/Order/Add", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json.Should().ContainKey("message");
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturnInternalServerError_OnException()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<ProductDto> { new ProductDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };

        _factory.OrderServiceMock
            .Setup(x => x.CreateOrder(It.IsAny<Order>(), It.IsAny<List<ProductDto>>()))
            .ThrowsAsync(new Exception(ErrorMessages.UnexpectedErrorWithMessage));

        // Act
        var response = await _client.PostAsJsonAsync("/api/Order/Order/Add", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    
    [Fact]
    public async Task CreateOrder_ShouldReturnBadRequest_WhenProductDtoIsEmpty()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<ProductDto>()
        };
    
        _factory.OrderServiceMock
            .Setup(x => x.CreateOrder(It.IsAny<Order>(), It.IsAny<List<ProductDto>>()))
            .Returns(Task.CompletedTask);
    
        // Act
        var response = await _client.PostAsJsonAsync("/api/Order/Order/Add", dto);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        json.Should().ContainKey("errors");

        var errors = JsonDocument.Parse(json["errors"]!.ToString()).RootElement;
        errors.ToString().Should().Contain(ValidationErrorMessages.ProductListNotEmpty);
        errors.ToString().Should().Contain(ValidationErrorMessages.RequiredProducts);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnOk_WithOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var fakeOrder = new Order
        {
            Id = orderId,
            OrderPersons = new List<OrderPerson>
            {
                new OrderPerson { PersonId = Guid.NewGuid() }
            }
        };

        _factory.OrderServiceMock
            .Setup(x => x.GetOrderById(orderId))
            .ReturnsAsync(fakeOrder);

        // Act
        var response = await _client.GetAsync($"/api/Order/Order/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var root = doc.RootElement;
        var returnedOrderId = root.GetProperty("id").GetGuid();

        returnedOrderId.Should().Be(orderId);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnBadRequest_WhenOrderIsNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _factory.OrderServiceMock
            .Setup(x => x.GetOrderById(orderId))!
            .ThrowsAsync(new BussinessArgumentException(ErrorMessages.OrderNotFound));

        // Act
        var response = await _client.GetAsync($"/api/Order/Order/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    
    [Fact]
    public async Task GetOrdersByRegionId_ShouldReturnOk_WithList()
    {
        // Arrange
        var regionId = 1;
        var fakeOrders = new List<Order> { new Order { Id = Guid.NewGuid() } };

        _factory.OrderServiceMock
            .Setup(x => x.GetOrdersByRegionId(regionId, null, false))
            .ReturnsAsync(fakeOrders);

        // Act
        var response = await _client.GetAsync($"/api/Order/Orders/{regionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var root = doc.RootElement;

        root.ValueKind.Should().Be(JsonValueKind.Object);
        root.TryGetProperty("$values", out var ordersArray).Should().BeTrue();
        ordersArray.ValueKind.Should().Be(JsonValueKind.Array);
        ordersArray.GetArrayLength().Should().Be(1);

    }

    [Fact]
    public async Task GetOrdersByRegionId_ShouldReturnInternalServerError_OnException()
    {
        // Arrange
        var regionId = 1;

        _factory.OrderServiceMock
            .Setup(x => x.GetOrdersByRegionId(regionId, null, false))
            .ThrowsAsync(new Exception(ErrorMessages.UnexpectedErrorWithMessage));

        // Act
        var response = await _client.GetAsync($"/api/Order/Orders/{regionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    [Fact]
    public async Task UpdateOrder_ShouldReturnOk_WithMessage()
    {
        // Arrange
        var dto = new UpdateOrderDto
        {
            Id = Guid.NewGuid(),
            Products = new List<ProductDto> { new ProductDto {ProductId = Guid.NewGuid(), Quantity = 1}}
        };

        _factory.OrderServiceMock
            .Setup(x => x.UpdateOrderProducts(It.IsAny<Order>(), It.IsAny<List<ProductDto>>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.PutAsJsonAsync("/api/Order/Order/Update", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json.Should().ContainKey("message");
    }

    [Fact]
    public async Task UpdateOrder_ShouldReturnInternalServerError_OnException()
    {
        // Arrange
        var dto = new UpdateOrderDto
        {
            Id = Guid.NewGuid(),
            Products = new List<ProductDto> { new ProductDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };

        _factory.OrderServiceMock
            .Setup(x => x.UpdateOrderProducts(It.IsAny<Order>(), It.IsAny<List<ProductDto>>()))
            .ThrowsAsync(new Exception(ErrorMessages.UnexpectedErrorWithMessage));

        // Act
        var response = await _client.PutAsJsonAsync("/api/Order/Order/Update", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task UpdateOrder_ShouldReturnBadRequest_WhenProductDtoIsEmpty()
    {
        // Arrange
        var dto = new UpdateOrderDto
        {
            Id = Guid.NewGuid(),
            Products = new List<ProductDto>()
        };

        _factory.OrderServiceMock
            .Setup(x => x.UpdateOrderProducts(It.IsAny<Order>(), It.IsAny<List<ProductDto>>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.PutAsJsonAsync("/api/Order/Order/Update", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        json.Should().ContainKey("errors");

        var errors = JsonDocument.Parse(json["errors"]!.ToString()).RootElement;
        errors.ToString().Should().Contain(ValidationErrorMessages.ProductListNotEmpty);
        errors.ToString().Should().Contain(ValidationErrorMessages.RequiredProducts);
    }
    
    [Fact]
    public async Task RemoveOrder_ShouldReturnOk_WithMessage()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _factory.OrderServiceMock
            .Setup(x => x.RemoveOrder(orderId))
            .Returns(Task.CompletedTask);

        _factory.DeliveryServiceMock
            .Setup(x => x.RemoveInvoice(orderId))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.DeleteAsync($"/api/Order/Order/Remove/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json!["message"].Should().Be(InfoMessages.RemovedOrder);
    }

    [Fact]
    public async Task RemoveOrder_ShouldReturnBadRequest_OnException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _factory.DeliveryServiceMock
            .Setup(x => x.RemoveInvoice(orderId))
            .ThrowsAsync(new BussinessArgumentException(ErrorMessages.InvoiceNotFound));

        // Act
        var response = await _client.DeleteAsync($"/api/Order/Order/Remove/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}