using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Enums;
using DeliveryProject.Bussiness.Mediators;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Dto;

namespace DeliveryProject.Bussiness.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly RepositoryMediator _repositoryMediator;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(RepositoryMediator repositoryMediator, ILogger<OrderService> logger, IMapper mapper)
        {
            _repositoryMediator = repositoryMediator;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Order> AddOrder(Order order, List<ProductDto> products)
        {
            var customer = await _repositoryMediator.GetAndValidateCustomerAsync(order.OrderPersons.First().PersonId);
            
            var orderProducts = await GetOrderProducts(order, products);

            var invoice = await GetInvoice(order, orderProducts);

            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                CreatedTime = DateTime.UtcNow,
                Status = OrderStatus.Active,
                OrderPersons = new List<OrderPersonEntity>
                {
                    new OrderPersonEntity { Person = customer }
                },
                OrderProducts = orderProducts,
                Invoice = invoice
            };

            var createdOrderEntity = await _repositoryMediator.AddOrderAsync(orderEntity);

            _logger.LogInformation(InfoMessages.AddedOrder, order.Id);

            return _mapper.Map<Order>(createdOrderEntity);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            var orderEntity = await _repositoryMediator.GetOrderByIdAsync(orderId);
            return _mapper.Map<Order>(orderEntity);
        }

        public async Task UpdateOrder(Order order, List<ProductDto> products)
        {
            var orderProducts = await GetOrderProducts(order, products);

            decimal amount = await CalculateOrderAmount(orderProducts);

            await _repositoryMediator.UpdateOrderAsync(order.Id, orderProducts, amount);

            _logger.LogInformation(InfoMessages.UpdatedOrder, order.Id);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            await _repositoryMediator.DeleteOrderAsync(orderId);
            _logger.LogInformation(InfoMessages.DeletedOrder, orderId);
        }

        public Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending)
        {
            return Task.Factory.StartNew(async () =>
            {
                var orders = await _repositoryMediator.GetAllOrdersImmediate();

                if (sortBy != null)
                {
                    var sortedOrders = GetSortDelegate(sortBy, descending);
                    orders = sortedOrders?.Invoke(orders) ?? orders;
                }

                _logger.LogInformation(InfoMessages.AllOrdersReceived, orders.Count);

                return _mapper.Map<List<Order>>(orders);
            }, TaskCreationOptions.LongRunning).Unwrap();
        }

        private async Task<List<OrderProductEntity>> GetOrderProducts(Order order, List<ProductDto> products)
        {
            var productEntities = await _repositoryMediator.GetAndValidateProductsAsync(
                products.Select(p => p.ProductId).Distinct().ToList());

            var orderProducts = products
                .Select(p => new OrderProductEntity
                {
                    OrderId = order.Id,
                    ProductId = p.ProductId,
                    Product = productEntities.First(pe => pe.Id == p.ProductId),
                    Quantity = p.Quantity
                }).ToList();

            return orderProducts;
        }

        private async Task<InvoiceEntity> GetInvoice(Order order, List<OrderProductEntity> orderProducts)
        {
            decimal amount = await CalculateOrderAmount(orderProducts);

            var deliveryTime = _repositoryMediator.CalculateDeliveryTime();

            return new InvoiceEntity
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Amount = amount,
                DeliveryTime = deliveryTime.ToUniversalTime(),
                IsExecuted = false
            };
        }

        private async Task<decimal> CalculateOrderAmount(List<OrderProductEntity> orderProducts)
        {
            return orderProducts.Sum(op => op.Product.Price * op.Quantity);
        }
    }
}
