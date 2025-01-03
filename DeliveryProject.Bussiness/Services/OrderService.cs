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
        private readonly ISupplierRepository _supplierRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger, IMapper mapper, 
            ISupplierRepository supplierRepository, IDeliveryPersonRepository deliveryPersonRepository)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
        }

        public async Task<Order> AddOrder(Order order, int supplierId)
        {
            var supplier = await _supplierRepository.GetByIdAsync(supplierId);
            if (supplier == null)
            {
                throw new BussinessArgumentException("The specified supplier was not found.", "SUPPLIER_NOT_FOUND");
            }

            var availableDeliveryPerson = await _deliveryPersonRepository.GetAvailableDeliveryPersonAsync(order.DeliveryTime);
            if (availableDeliveryPerson == null)
            {
                throw new BussinessArgumentException("There are no available delivery persons at the specified time.", "NO_AVAILABLE_DELIVERYPERSONS");
            }

            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                Weight = order.Weight,
                RegionId = order.RegionId,
                DeliveryTime = order.DeliveryTime,
                SupplierId = supplierId,
                DeliveryPersonId = availableDeliveryPerson.Id
            };

            await _orderRepository.AddOrder(orderEntity);

            availableDeliveryPerson.DeliverySlots.Add(order.DeliveryTime);
            await _deliveryPersonRepository.UpdateAsync(availableDeliveryPerson);

            _logger.LogInformation($"Added an order with an ID: {order.Id}.");

            return _mapper.Map<Order>(orderEntity);
        }

        public async Task<List<Order>> FilterOrders(string? regionName)
        {
            if(string.IsNullOrEmpty(regionName))
            {
                throw new BussinessArgumentException($"The regionName field must not be empty.", "REGION_MUST_NOT_BE_EMPTY");
            }

            var region = await _orderRepository.GetRegionByName(regionName);
            if (region == null)
            {
                throw new BussinessArgumentException($"The region with the name {regionName} was not found.", "REGION_NOT_FOUND");
            }

            var regionId = region.Id;

            var hasOrders = await _orderRepository.HasOrders(regionId);
            if (!hasOrders)
            {
                throw new BussinessArgumentException($"No orders were found in this area ({regionId}).",
            "ORDERS_NOT_FOUND_IN_REGION");
            }

            var firstOrderTime = await _orderRepository.GetFirstOrderTime(regionId);
            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(regionId, firstOrderTime, timeRangeEnd);
            if (filteredOrders == null || filteredOrders.Count == 0)
            {
                throw new BussinessArgumentException(
                    $"No orders were found for the {regionId} area in the time range from {firstOrderTime} to {timeRangeEnd}.",
                    "ORDERS_IN_TIME_RANGE_NOT_FOUND");
            }

            _logger.LogInformation(
                $"{filteredOrders.Count} orders found for the {regionId} area in the range from {firstOrderTime} to {timeRangeEnd}.");

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
                throw new BussinessArgumentException("No orders found.", "NO_ORDERS_FOUND");
            }

            _logger.LogInformation($"{orders.Count} orders received.");

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
