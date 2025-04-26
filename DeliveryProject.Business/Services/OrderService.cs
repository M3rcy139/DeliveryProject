using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DeliveryProject.Business.Extensions;
using DeliveryProject.Business.Interfaces.Services;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(
            IUnitOfWork unitOfWork,
            ILogger<OrderService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Order> AddOrder(Order order, List<ProductDto> products)
        {
            return await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var customer = await _unitOfWork.Customers.GetCustomerById(order.OrderPersons.First().PersonId);
                customer.ValidateEntity(ErrorMessages.CustomerNotFound, ErrorCodes.CustomerNotFound);

                var orderProducts = await GetOrderProducts(order, products);
                var amount = orderProducts.CalculateOrderAmount();

                var orderEntity = new OrderEntity()
                {
                    Id = order.Id,
                    CreatedTime = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    Amount = amount,
                    OrderPersons = new List<OrderPersonEntity>
                    {
                        new OrderPersonEntity { Person = customer }
                    },
                    OrderProducts = orderProducts,
                };

                await _unitOfWork.Orders.AddOrder(orderEntity);

                _logger.LogInformation(InfoMessages.AddedOrder, order.Id);
                return _mapper.Map<Order>(orderEntity);
            }, _logger);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            return await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var orderEntity = await _unitOfWork.Orders.GetOrderById(orderId);
                orderEntity.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);
            
                return _mapper.Map<Order>(orderEntity);
            }, _logger);
            
        }

        public async Task UpdateOrderProducts(Order order, List<ProductDto> products)
        {
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var updatedOrder = await _unitOfWork.Orders.GetOrderById(order.Id);
                updatedOrder.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

                var orderProducts = await GetOrderProducts(order, products);
                decimal amount = orderProducts.CalculateOrderAmount();

                updatedOrder!.Amount = amount;
                updatedOrder.OrderProducts.Clear();
                updatedOrder.OrderProducts.AddRange(orderProducts.Select(op =>
                    new OrderProductEntity
                    {
                        ProductId = op.ProductId,
                        OrderId = op.OrderId,
                        Quantity = op.Quantity
                    }));

                await _unitOfWork.Orders.UpdateOrderProducts(updatedOrder);

                _logger.LogInformation(InfoMessages.UpdatedOrder, updatedOrder.Id);
            }, _logger);
        }

        public async Task UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var order = await _unitOfWork.Orders.GetOrderById(orderId);
                order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

                order!.Status = status;

                await _unitOfWork.Orders.UpdateOrderStatus(order);

                _logger.LogInformation(InfoMessages.UpdatedOrderStatus, order.Id);
            }, _logger);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var order = await _unitOfWork.Orders.GetOrderById(orderId);
                order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

                await _unitOfWork.Orders.DeleteOrder(orderId);
                _logger.LogInformation(InfoMessages.DeletedOrder, orderId);
            }, _logger);
        }

        public Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending)
        {
            return Task.Factory.StartNew(async () =>
            {
                var orders = await _unitOfWork.ExecuteInTransaction(async () =>
                {
                    var ordersList = await _unitOfWork.Orders.GetAllOrders();
            
                    if (sortBy != null)
                    {
                        var sortedOrders = GetSortDelegate(sortBy, descending);
                        ordersList = sortedOrders?.Invoke(ordersList) ?? ordersList;
                    }

                    return ordersList.IsNullOrEmpty() ? new List<OrderEntity>() : ordersList;
                }, _logger);

                _logger.LogInformation(InfoMessages.AllOrdersReceived, orders.Count);

                return _mapper.Map<List<Order>>(orders);
            }, TaskCreationOptions.LongRunning).Unwrap();
        }

        private async Task<List<OrderProductEntity>> GetOrderProducts(Order order, List<ProductDto> products)
        {
            var productEntities = await _unitOfWork.Products.GetProductsById(
                products.Select(p => p.ProductId).Distinct().ToList());
            productEntities.ValidateEntities(ErrorMessages.ProductNotFound, ErrorCodes.ProductNotFound);

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
