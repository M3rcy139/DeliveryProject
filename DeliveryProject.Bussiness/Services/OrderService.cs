using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Core.Constants;
using DeliveryProject.Bussiness.Helpers;
using DeliveryProject.Bussiness.Mediators;

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

        public async Task<Order> AddOrder(Order order)
        {
            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                Weight = order.Weight,
                RegionId = order.RegionId,
                DeliveryTime = order.DeliveryTime,
                SupplierId = order.SupplierId,
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

        public async Task UpdateOrder(Order order)
        {
            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                Weight = order.Weight,
                RegionId = order.RegionId,
                DeliveryTime = order.DeliveryTime,
                SupplierId = order.SupplierId
            };

            await _repositoryMediator.UpdateOrderAsync(orderEntity);

            _logger.LogInformation(InfoMessages.UpdatedOrder, order.Id);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            await _repositoryMediator.DeleteOrderAsync(orderId);
            _logger.LogInformation(InfoMessages.DeletedOrder, orderId);
        }
        public async Task<List<Order>> FilterOrders(string? regionName)
        {
            var region = await _repositoryMediator.GetRegionByNameAsync(regionName);

            var firstOrderTime = await _repositoryMediator.GetFirstOrderTimeAsync(region.Id);
            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _repositoryMediator.GetOrdersWithinTimeRangeAsync(region.Id, firstOrderTime, timeRangeEnd);

            _logger.LogInformation(
                InfoMessages.FoundInRegion, filteredOrders.Count, region.Id, firstOrderTime, timeRangeEnd);

            return _mapper.Map<List<Order>>(filteredOrders);
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

                    GC.Collect();
                }

                _logger.LogInformation(InfoMessages.AllOrdersReceived, orders.Count);

                return _mapper.Map<List<Order>>(orders);
            }, TaskCreationOptions.LongRunning).Unwrap();
        }
    }
}
