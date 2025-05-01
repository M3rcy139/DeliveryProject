using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DeliveryProject.Business.Extensions;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Business.Mediators;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Dto;
using DeliveryProject.Business.Helpers;

namespace DeliveryProject.Business.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly MediatorHelper<OrderEntity> _orderMediator;
        private readonly MediatorHelper<CustomerEntity> _customerMediator;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(
            MediatorHelper<OrderEntity> orderMediator,
            MediatorHelper<CustomerEntity> customerMediator,
            ILogger<OrderService> logger,
            IMapper mapper)
        {
            _orderMediator = orderMediator;
            _customerMediator = customerMediator;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Order> AddOrder(Order order, List<ProductDto> products)
        {
            var customer = await _customerMediator.GetEntityById(order.OrderPersons.First().PersonId);
            
            var orderProducts = await GetOrderProducts(order, products);

            var amount = orderProducts.CalculateOrderAmount();
            
            var orderEntity = BuildEntityHelper.BuildNewOrderEntity(order, customer!, orderProducts, amount);
            
            await _orderMediator.AddEntity(orderEntity);

            _logger.LogInformation(InfoMessages.AddedOrderDetail + "{@OrderEntity}.", orderEntity);

            return _mapper.Map<Order>(orderEntity);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            var orderEntity = await _orderMediator.GetEntityById(orderId);
            
            return _mapper.Map<Order>(orderEntity);
        }

        public async Task UpdateOrderProducts(Order order, List<ProductDto> products)
        {
            var updatedOrder = await _orderMediator.GetEntityById(order.Id);
            
            var orderProducts = await GetOrderProducts(order, products);
            
            decimal amount = orderProducts.CalculateOrderAmount();
            
            BuildEntityHelper.BuildUpdatedOrderEntity(updatedOrder!, orderProducts, amount);
            
            await _orderMediator.UpdateOrderProducts(updatedOrder);

            _logger.LogInformation(InfoMessages.UpdatedOrderDetail + "{@OrderEntity}.", updatedOrder.Id);
        }

        public async Task UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            var order = await _orderMediator.GetEntityById(orderId);
            
            order.Status = status;
            
            await _orderMediator.UpdateOrderStatus(order);
            
            _logger.LogInformation(InfoMessages.UpdatedOrderStatusDetail + "{@OrderEntity}.", order.Id);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            await _orderMediator.DeleteEntityById(orderId);
            
            _logger.LogInformation(InfoMessages.DeletedOrderDetail + "{@OrderEntity}.", orderId);
        }

        public Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending)
        {
            return Task.Factory.StartNew(async () =>
            {
                var orders = await _orderMediator.GetAllOrders();

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
            var productEntities = await _orderMediator.GetProductsByIds(
                products.Select(p => p.ProductId).Distinct().ToList());

            var orderProducts = BuildEntityHelper
                .BuildOrderProductsEntity(order, productEntities, products);

            return orderProducts;
        }
    }
}
