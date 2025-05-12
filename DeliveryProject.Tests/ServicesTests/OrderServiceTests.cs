using AutoMapper;
using DeliveryProject.Business.Services;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Settings;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Tests.Extensions;
using DeliveryProject.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DeliveryProject.Tests.ServicesTests
{
    public class OrderServiceTests
    {
        private readonly MediatorMockFactory _mockFactory;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IOptions<OrderSettings> _orderSettings;

        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockFactory = new MediatorMockFactory();
            _loggerMock = new Mock<ILogger<OrderService>>();
            _mapperMock = new Mock<IMapper>();
            _orderSettings = Options.Create(new OrderSettings());

            var orderMediator = _mockFactory.CreateMediator<OrderEntity>();
            var customerMediator = _mockFactory.CreateMediator<CustomerEntity>();

            _service = new OrderService(
                orderMediator,
                customerMediator,
                _loggerMock.Object,
                _mapperMock.Object,
                _orderSettings
            );
        }

        [Fact]
        public async Task CreateOrder_ShouldAddOrder()
        {
            // Arrange
            var order = new Order
            {
                OrderPersons = new List<OrderPerson>
                {
                    new() { PersonId = Guid.NewGuid() }
                }
            };
            var products = new List<ProductDto>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 1 }
            };

            var customer = new CustomerEntity();
            var productEntities = new List<ProductEntity> { new() { Id = products[0].ProductId, Price = 10 } };

            _mockFactory.CustomerRepositoryMock
                .Setup(repo => repo.GetCustomerById(It.IsAny<Guid>()))
                .ReturnsAsync(customer);

            _mockFactory.ProductRepositoryMock
                .Setup(repo => repo.GetProductsById(It.IsAny<List<Guid>>()))!
                .ReturnsAsync(productEntities);

            _mockFactory.OrderRepositoryMock
                .Setup(repo => repo.AddOrder(It.IsAny<OrderEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateOrder(order, products);

            // Assert
            _mockFactory.OrderRepositoryMock.Verify(repo => repo.AddOrder(It.IsAny<OrderEntity>()), Times.Once);
            _loggerMock.VerifyLogContains(InfoMessages.AddedOrderDetail);
        }

        [Fact]
        public async Task CreateOrder_ShouldThrow_WhenCustomerNotFound()
        {
            // Arrange
            var order = new Order
            {
                OrderPersons = new List<OrderPerson> { new() { PersonId = Guid.NewGuid() } }
            };
            var products = new List<ProductDto> { new() { ProductId = Guid.NewGuid(), Quantity = 1 } };

            _mockFactory.CustomerRepositoryMock
                .Setup(r => r.GetCustomerById(It.IsAny<Guid>()))
                .ReturnsAsync((CustomerEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.CreateOrder(order, products));
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnMappedOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var entity = new OrderEntity { Id = orderId };
            var dto = new Order { Id = orderId };

            _mockFactory.OrderRepositoryMock
                .Setup(repo => repo.GetOrderById(orderId))
                .ReturnsAsync(entity);

            _mapperMock
                .Setup(mapper => mapper.Map<Order>(entity))
                .Returns(dto);

            // Act
            var result = await _service.GetOrderById(orderId);

            // Assert
            Assert.Equal(orderId, result.Id);
        }

        [Fact]
        public async Task GetOrderById_ShouldThrow_WhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.GetOrderById(orderId))
                .ReturnsAsync((OrderEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.GetOrderById(orderId));
        }

        [Fact]
        public async Task GetOrdersByRegionId_ShouldReturnSortedOrders()
        {
            // Arrange
            var regionId = 1;
            var orders = new List<OrderEntity>
            {
                new() { Id = Guid.NewGuid(), CreatedTime = DateTime.UtcNow }
            };

            _mockFactory.OrderRepositoryMock
                .Setup(repo => repo.GetOrdersByRegionId(regionId))
                .ReturnsAsync(orders);

            _mapperMock
                .Setup(mapper => mapper.Map<List<Order>>(orders))
                .Returns(new List<Order> { new() });

            // Act
            var result = await _service.GetOrdersByRegionId(regionId, OrderSortField.CreatedTime, false);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetOrdersByRegionId_ShouldReturnEmpty_WhenRepositoryReturnsEmpty()
        {
            // Arrange
            var regionId = 10;

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.GetOrdersByRegionId(regionId))
                .ReturnsAsync(new List<OrderEntity>());

            _mapperMock
                .Setup(m => m.Map<List<Order>>(It.IsAny<List<OrderEntity>>()))
                .Returns(new List<Order>());

            // Act
            var result = await _service.GetOrdersByRegionId(regionId, null, false);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateOrderProducts_ShouldUpdateSuccessfully()
        {
            // Arrange
            var order = new Order { Id = Guid.NewGuid() };
            var entity = new OrderEntity { Id = order.Id };
            var productDto = new ProductDto { ProductId = Guid.NewGuid(), Quantity = 1 };
            var productEntity = new ProductEntity { Id = productDto.ProductId, Price = 10 };

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.GetOrderById(order.Id))
                .ReturnsAsync(entity);

            _mockFactory.ProductRepositoryMock
                .Setup(p => p.GetProductsById(It.IsAny<List<Guid>>()))!
                .ReturnsAsync(new List<ProductEntity> { productEntity });

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.UpdateOrderProducts(entity))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateOrderProducts(order, new List<ProductDto> { productDto });

            // Assert
            _mockFactory.OrderRepositoryMock.Verify(r => r.UpdateOrderProducts(It.IsAny<OrderEntity>()), Times.Once);
            _loggerMock.VerifyLogContains(InfoMessages.UpdatedOrderDetail);
        }

        [Fact]
        public async Task UpdateOrderProducts_ShouldThrow_WhenOrderNotFound()
        {
            // Arrange
            var order = new Order { Id = Guid.NewGuid() };
            var products = new List<ProductDto> { new() { ProductId = Guid.NewGuid(), Quantity = 2 } };

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.GetOrderById(order.Id))
                .ReturnsAsync((OrderEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.UpdateOrderProducts(order, products));
        }

        [Fact]
        public async Task UpdateOrderStatus_ShouldUpdateStatus()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var entity = new OrderEntity { Id = orderId };

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.GetOrderById(orderId))
                .ReturnsAsync(entity);

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.UpdateOrderStatus(entity));

            // Act
            await _service.UpdateOrderStatus(orderId, OrderStatus.Completed);

            // Assert
            Assert.Equal(OrderStatus.Completed, entity.Status);
            _loggerMock.VerifyLogContains(InfoMessages.UpdatedOrderStatusDetail);
        }

        [Fact]
        public async Task UpdateOrderStatus_ShouldThrow_WhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.GetOrderById(orderId))
                .ReturnsAsync((OrderEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.UpdateOrderStatus(orderId, OrderStatus.Completed));
        }

        [Fact]
        public async Task RemoveOrder_ShouldCallRepository()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var entity = new OrderEntity { Id = orderId };

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.GetOrderById(orderId))
                .ReturnsAsync(entity);

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.RemoveOrder(orderId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.RemoveOrder(orderId);

            // Assert
            _mockFactory.OrderRepositoryMock.Verify(r => r.RemoveOrder(orderId), Times.Once);
            _loggerMock.VerifyLogContains(InfoMessages.RemovedOrder);
        }

        [Fact]
        public async Task RemoveOrder_ShouldThrow_WhenRepositoryThrows()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockFactory.OrderRepositoryMock
                .Setup(r => r.RemoveOrder(orderId))
                .ThrowsAsync(new BussinessArgumentException(ErrorMessages.OrderNotFound));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BussinessArgumentException>(() => _service.RemoveOrder(orderId));
            Assert.Equal(ErrorMessages.OrderNotFound, ex.Message);
        }
    }
}
