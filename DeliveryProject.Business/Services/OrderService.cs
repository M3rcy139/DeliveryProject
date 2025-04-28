using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DeliveryProject.Business.Extensions;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Business.Mediators;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Extensions;

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
            
            var orderEntity = BuildNewOrderEntity(order, customer!, orderProducts, amount);
            
            await _orderMediator.AddEntity(orderEntity);

            _logger.LogInformation(InfoMessages.AddedOrder, order.Id);

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
            
            BuildUpdatedOrderEntity(updatedOrder!, orderProducts, amount);
            
            await _orderMediator.UpdateOrderProducts(updatedOrder);

            _logger.LogInformation(InfoMessages.UpdatedOrder, updatedOrder.Id);
        }

        public async Task UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            var order = await _orderMediator.GetEntityById(orderId);
            
            order.Status = status;
            
            await _orderMediator.UpdateOrderStatus(order);
            
            _logger.LogInformation(InfoMessages.UpdatedOrderStatus, order.Id);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            await _orderMediator.DeleteEntityById(orderId);
            
            _logger.LogInformation(InfoMessages.DeletedOrder, orderId);
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

        private OrderEntity BuildNewOrderEntity(Order order,
            CustomerEntity customer,
            List<OrderProductEntity> orderProducts,
            decimal amount)
        {
            return new OrderEntity
            {
                Id = order.Id,
                CreatedTime = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Amount = amount,
                OrderPersons = new List<OrderPersonEntity>
                {
                    new OrderPersonEntity { Person = customer }
                },
                OrderProducts = orderProducts
            };
        }

        private void BuildUpdatedOrderEntity(OrderEntity updatedOrder, List<OrderProductEntity> orderProducts, 
            decimal amount)
        {
            updatedOrder!.Amount = amount;
            updatedOrder.OrderProducts.Clear();
            updatedOrder.OrderProducts.AddRange(orderProducts.Select(op =>
                new OrderProductEntity
                {
                    ProductId = op.ProductId,
                    OrderId = op.OrderId,
                    Quantity = op.Quantity
                }));
        }
        
        private async Task<List<OrderProductEntity>> GetOrderProducts(Order order, List<ProductDto> products)
        {
            var productEntities = await _orderMediator.GetProductsByIds(
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
