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
        private readonly IDeliveryService _deliveryService;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(RepositoryMediator repositoryMediator, IDeliveryService deliveryService ,ILogger<OrderService> logger, IMapper mapper)
        {
            _repositoryMediator = repositoryMediator;
            _deliveryService = deliveryService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Order> AddOrder(Order order, List<ProductDto> products)
        {
            var customer = await _repositoryMediator.GetCustomerById(order.OrderPersons.First().PersonId);
            
            var orderProducts = await GetOrderProducts(order, products);

            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                CreatedTime = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderPersons = new List<OrderPersonEntity>
                {
                    new OrderPersonEntity { Person = customer }
                },
                OrderProducts = orderProducts,
            };
            
            var createdOrderEntity = await _repositoryMediator.AddOrder(orderEntity);

            _logger.LogInformation(InfoMessages.AddedOrder, order.Id);

            return _mapper.Map<Order>(createdOrderEntity);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            var orderEntity = await _repositoryMediator.GetOrderById(orderId);
            return _mapper.Map<Order>(orderEntity);
        }

        public async Task UpdateOrder(Order order, List<ProductDto> products)
        {
            var orderProductsEntity = await GetOrderProducts(order, products);
            
            var orderProducts = _mapper.Map<List<OrderProduct>>(orderProductsEntity);
            
            await _repositoryMediator.UpdateOrder(order.Id, orderProductsEntity);

            _logger.LogInformation(InfoMessages.UpdatedOrder, order.Id);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            await _deliveryService.DeleteInvoice(orderId);
            await _repositoryMediator.DeleteOrder(orderId);
            _logger.LogInformation(InfoMessages.DeletedOrder, orderId);
        }

        public Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending)
        {
            return Task.Factory.StartNew(async () =>
            {
                var orders = await _repositoryMediator.GetAllOrders();

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
            var productEntities = await _repositoryMediator.GetProductsByIds(
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
    }
}
