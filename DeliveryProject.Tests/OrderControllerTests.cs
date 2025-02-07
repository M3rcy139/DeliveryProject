using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Dto;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Bussiness.Services;
using DeliveryProject.Middleware;
using DeliveryProject.Tests.Mocks;
using Newtonsoft.Json;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Repositories;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DeliveryProject.Tests.Base;
using DeliveryProject.Tests.Assertions;
using Microsoft.Extensions.Logging;

public class OrderControllerTests : BaseControllerTests, IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ILogger<OrderControllerTests> _logger;

    public OrderControllerTests(WebApplicationFactory<Program> factory)
        : base(
            InitializeClient(factory, out var orderServiceMock, out var orderRepositoryMock),
            orderServiceMock,
            orderRepositoryMock)
    {
        using var scope = factory.Services.CreateScope();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderControllerTests>>();
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
                services.AddSingleton<IOrderService>(_ => localOrderServiceMock.Object);
                services.AddSingleton<IOrderRepository>(_ => localOrderRepositoryMock.Object);
                services.AddLogging();
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
        var request = new OrderViewModel
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

        ControllerAssertions.AssertResponseDetails(responseContent, "Validation failed.", nameof(ValidationMiddleware));
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

        ControllerAssertions.AssertResponseDetails(responseContent, "The RegionName field must not be empty.", nameof(OrderService.FilterOrders));
    }

    [Theory]
    [InlineData(30)]
    public async Task UpdateOrder_ParallelRequests_ShouldHandleCacheLocking(int parallelRequests)
    {
        _logger.LogInformation("Starting UpdateOrder_ParallelRequests test with {ParallelRequests} parallel requests", parallelRequests);


        // Arrange
        var options = new DbContextOptionsBuilder<DeliveryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var factory = new PooledDbContextFactory<DeliveryDbContext>(options);
        var repository = new OrderRepository(factory);

        using var dbContext = factory.CreateDbContext();

        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            Weight = 10,
            RegionId = 2,
            SupplierId = 1,
            DeliveryTime = DateTime.UtcNow
        };

        await repository.AddOrder(order);
        _logger.LogDebug("Order with ID {OrderId} added", order.Id);

        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < parallelRequests; i++)
        {
            var index = i;
            var task = Task.Run(async () =>
            {
                try
                {
                    var updatedWeight = 10 + index;
                    var threadId = Environment.CurrentManagedThreadId;
                    _logger.LogDebug("Thread {ThreadId} updating order {OrderId} to weight {Weight}", threadId, order.Id, updatedWeight);

                    var updatedOrder = new OrderEntity
                    {
                        Id = order.Id,
                        Weight = updatedWeight,
                        RegionId = order.RegionId,
                        SupplierId = order.SupplierId,
                        DeliveryTime = order.DeliveryTime
                    };

                    await repository.UpdateOrder(updatedOrder);
                    _logger.LogDebug("Order {OrderId} updated to weight {Weight}", order.Id, updatedWeight);

                    var fetchedOrder = await repository.GetOrderById(order.Id);
                    fetchedOrder.Should().NotBeNull();
                    fetchedOrder.Weight.Should().Be(updatedWeight);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating order in parallel execution");
                }
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        // Assert
        var finalUpdatedOrder = await repository.GetOrderById(order.Id);
        finalUpdatedOrder.Should().NotBeNull();
        finalUpdatedOrder.Weight.Should().Be(9 + (parallelRequests));

        _logger.LogInformation("Finished UpdateOrder_ParallelRequests test.");
    }

    [Theory]
    [InlineData(30)]
    public async Task GetAllOrdersImmediate_ParallelAccess_ShouldBeThreadSafe(int parallelRequests)
    {
        _logger.LogInformation("Starting GetAllOrdersImmediate_ParallelAccess test with {ParallelRequests} parallel requests", parallelRequests);

        // Arrange
        var options = new DbContextOptionsBuilder<DeliveryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var factory = new PooledDbContextFactory<DeliveryDbContext>(options);

        using (var dbContext = factory.CreateDbContext())
        {
            var initialOrders = new List<OrderEntity>();
            for (int i = 0; i < 50; i++)
            {
                initialOrders.Add(new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    Weight = i,
                    RegionId = 1,
                    SupplierId = 1,
                    DeliveryTime = DateTime.UtcNow
                });
            }
            await dbContext.Orders.AddRangeAsync(initialOrders);
            await dbContext.SaveChangesAsync();
        }

        _logger.LogDebug("50 test orders added to in-memory DB");
        
        var sharedOrders = new List<OrderEntity>();
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < parallelRequests; i++)
        {
            var index = i;
            var task = Task.Run(async () =>
            {
                try
                {
                    var threadId = Environment.CurrentManagedThreadId;
                    _logger.LogDebug("Thread {ThreadId} fetching all orders, request {Index}", threadId, index);

                    var scopedRepository = new OrderRepository(factory);
                    var orders = await scopedRepository.GetAllOrdersImmediate();

                    sharedOrders.AddRange(orders);

                    _logger.LogDebug("Thread {ThreadId} finished fetching orders, request {Index}", threadId, index);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching orders in parallel execution");
                }
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        // Assert
        var uniqueOrders = sharedOrders.Select(o => o.Id).Distinct().ToList();
        uniqueOrders.Should().HaveCount(50);

        foreach (var order in sharedOrders)
        {
            order.Should().NotBeNull();
            order.Id.Should().NotBe(Guid.Empty);
            order.Weight.Should().BeInRange(0, 49);
            order.RegionId.Should().Be(1);
            order.SupplierId.Should().Be(1);
        }

        _logger.LogInformation("Finished GetAllOrdersImmediate_ParallelAccess test.");
    }
}
