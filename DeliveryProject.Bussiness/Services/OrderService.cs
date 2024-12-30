using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using DeliveryProject.Core.Exceptions;
using AutoMapper;
using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Bussiness.Enums;

namespace DeliveryProject.Bussiness.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> AddOrder(Order order)
        {
            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                Weight = order.Weight,
                RegionId = order.RegionId,
                DeliveryTime = order.DeliveryTime,
            };

            var result = await _orderRepository.AddOrder(orderEntity);

            _logger.LogInformation($"Добавлен заказ с ID: {order.Id}");

            return result;
        }

        public async Task<List<Order>> FilterOrders(string regionName)
        {
            var region = await _orderRepository.GetRegionByName(regionName);
            if (region == null)
            {
                throw new BussinessArgumentException($"Регион с названием {regionName} не найден.", "REGION_NOT_FOUND");
            }

            var regionId = region.Id;

            var hasOrders = await _orderRepository.HasOrders(regionId);
            if (!hasOrders)
            {
                throw new BussinessArgumentException($"Заказов в данном районе({regionId}) не найдено",
            "ORDERS_NOT_FOUND_IN_REGION");
            }

            var firstOrderTime = await _orderRepository.GetFirstOrderTime(regionId);
            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(regionId, firstOrderTime, timeRangeEnd);
            if (filteredOrders == null || filteredOrders.Count == 0)
            {
                throw new BussinessArgumentException(
                    $"Заказы не найдены для района {regionId} в диапазоне времени с {firstOrderTime} по {timeRangeEnd}",
                    "ORDERS_IN_TIME_RANGE_NOT_FOUND");
            }

            _logger.LogInformation(
                $"Найдено {filteredOrders.Count} заказов для района {regionId} в диапазоне от {firstOrderTime} до {timeRangeEnd}");

            return _mapper.Map<List<Order>>(filteredOrders);
        }

        public async Task<List<Order>> GetAllOrders(SortField? sortBy, bool descending)
        {
            List<OrderEntity> orders;

            if (sortBy != null)
            {
                var ordersQuery = _orderRepository.GetAllOrdersDeferred();

                var sortedOrders = GetOrdersByDelegate(sortBy, descending);

                if (sortedOrders != null)
                {
                    ordersQuery = sortedOrders(ordersQuery);
                }

                orders = await ordersQuery.ToListAsync();
            }
            else
            {
                orders = await _orderRepository.GetAllOrdersImmediate();
            }

            if (orders == null || orders.Count == 0)
            {
                throw new BussinessArgumentException("Заказы не найдены", "NO_ORDERS_FOUND");
            }

            _logger.LogInformation($"Получено {orders.Count} заказов");

            return _mapper.Map<List<Order>>(orders);
        }

        private Func<IQueryable<OrderEntity>, IOrderedQueryable<OrderEntity>>? GetOrdersByDelegate(SortField? sortBy, bool descending)
        {
            return sortBy switch
            {
                SortField.Weight => q => descending ? q.OrderByDescending(o => o.Weight) : q.OrderBy(o => o.Weight),
                SortField.RegionId => q => descending ? q.OrderByDescending(o => o.RegionId) : q.OrderBy(o => o.RegionId),
                SortField.DeliveryTime => q => descending ? q.OrderByDescending(o => o.DeliveryTime) : q.OrderBy(o => o.DeliveryTime),
                _ => null
            };
        }

    }
}
