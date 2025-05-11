using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DeliveryProject.Business.Extensions;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Business.Mediators;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Extensions;
using DeliveryProject.Core.Settings;
using Microsoft.Extensions.Options;

namespace DeliveryProject.Business.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly Mediator<OrderEntity> _orderMediator;
        private readonly Mediator<CustomerEntity> _customerMediator;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;
        private readonly OrderSettings _orderSettings;

        public OrderService(
            Mediator<OrderEntity> orderMediator,
            Mediator<CustomerEntity> customerMediator,
            ILogger<OrderService> logger,
            IMapper mapper,
            IOptions<OrderSettings> orderSettings)
        {
            _orderMediator = orderMediator;
            _customerMediator = customerMediator;
            _logger = logger;
            _mapper = mapper;
            _orderSettings = orderSettings.Value;
        }

        public async Task CreateOrder(Order order, List<ProductDto> products)
        {
            var customer = await _customerMediator.GetEntityById(order.OrderPersons.First().PersonId);
            var orderProducts = await GetOrderProducts(order, products);
            var amount = orderProducts.CalculateOrderAmount();
            
            var orderEntity = BuildEntity.BuildNewOrderEntity(order, customer!, orderProducts, amount);
            await _orderMediator.AddEntity(orderEntity);

            _logger.LogInformation(InfoMessages.AddedOrderDetail + "{@OrderEntity}.", orderEntity);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            var orderEntity = await _orderMediator.GetEntityById(orderId);
            
            return _mapper.Map<Order>(orderEntity);
        }

        public Task<List<Order>> GetOrdersByRegionId(int regionId, OrderSortField? sortBy, bool descending)
        {
            return Task.Factory.StartNew(async () =>
            {
                var orders = await _orderMediator.GetOrdersByRegionId(regionId);

                if (sortBy != null)
                {
                    var sortedOrders = GetSortDelegate(sortBy, descending);
                    orders = sortedOrders?.Invoke(orders) ?? orders;
                }

                _logger.LogInformation(InfoMessages.AllOrdersReceived, orders.Count);

                return _mapper.Map<List<Order>>(orders);
            }, TaskCreationOptions.LongRunning).Unwrap();
        }

        public async Task UpdateOrderProducts(Order order, List<ProductDto> products)
        {
            var updatedOrder = await _orderMediator.GetEntityById(order.Id);
            
            var orderProducts = await GetOrderProducts(order, products);
            
            decimal amount = orderProducts.CalculateOrderAmount();
            
            BuildEntity.BuildUpdatedOrderEntity(updatedOrder!, orderProducts, amount);
            
            await _orderMediator.UpdateOrderProducts(updatedOrder);

            _logger.LogInformation(InfoMessages.UpdatedOrderDetail + "{@OrderEntity}.", updatedOrder);
        }

        public async Task UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            var order = await _orderMediator.GetEntityById(orderId);
            
            order.Status = status;
            
            _orderMediator.UpdateOrderStatus(order);
            
            _logger.LogInformation(InfoMessages.UpdatedOrderStatusDetail + "{@OrderEntity}.", order);
        }

        public async Task RemoveOrder(Guid orderId)
        {
            await _orderMediator.RemoveEntityById(orderId);
            
            _logger.LogInformation(InfoMessages.RemovedOrder);
        }

        private async Task<List<OrderProductEntity>> GetOrderProducts(Order order, List<ProductDto> products)
        {
            var productEntities = await _orderMediator.GetProductsByIds(
                products.Select(p => p.ProductId).Distinct().ToList());
            
            var orderProducts = BuildEntity
                .BuildOrderProductsEntity(order, productEntities, products);

            return orderProducts;
        }
    }
}
