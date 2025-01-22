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
    public class OrderService : IOrderService
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

        public async Task<Order> AddOrder(Order order, int supplierId)
        {
            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                Weight = order.Weight,
                RegionId = order.RegionId,
                DeliveryTime = order.DeliveryTime,
                SupplierId = supplierId,
            };

            var createdOrderEntity = await _repositoryMediator.AddOrderAsync(orderEntity);

            _logger.LogInformation(InfoMessages.AddedOrder, order.Id);

            return _mapper.Map<Order>(createdOrderEntity);
        }

        public async Task<List<Order>> FilterOrders(string? regionName)
        {
            var region = await _repositoryMediator.GetRegionByNameAsync(regionName);

            await _repositoryMediator.HasOrdersAsync(region.Id);

            var firstOrderTime = await _repositoryMediator.GetFirstOrderTimeAsync(region.Id);
            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _repositoryMediator.GetOrdersWithinTimeRangeAsync(region.Id, firstOrderTime, timeRangeEnd);

            _logger.LogInformation(
                InfoMessages.FoundInRegion, filteredOrders.Count, region.Id, firstOrderTime, timeRangeEnd);

            return _mapper.Map<List<Order>>(filteredOrders);
        }

        public async Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending)
        {
            var orders = await _repositoryMediator.GetAllOrdersImmediate();

            if (sortBy != null)
            {
                var sortedOrders = OrderServiceHelper.GetSortDelegate(sortBy, descending);

                orders = sortedOrders?.Invoke(orders) ?? orders;
            }

            _logger.LogInformation(InfoMessages.AllOrdersReceived, orders.Count);

            return _mapper.Map<List<Order>>(orders);
        }
    }
}
