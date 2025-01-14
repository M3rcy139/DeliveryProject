using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Models;
using Microsoft.Extensions.Logging;
using DeliveryProject.Core.Exceptions;
using AutoMapper;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Core.Constants;
using DeliveryProject.Bussiness.Helpers;
using DeliveryProject.Bussiness.Facades;

namespace DeliveryProject.Bussiness.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepositoryFacade _repositoryFacade;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(OrderRepositoryFacade repositoryFacade, ILogger<OrderService> logger, IMapper mapper)
        {
            _repositoryFacade = repositoryFacade;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Order> AddOrder(Order order, int supplierId)
        {
            var supplier = await _repositoryFacade.GetSupplierByIdAsync(supplierId);
            if (supplier == null)
            {
                throw new BussinessArgumentException(ErrorMessages.Supplier.NotFound, ErrorCodes.Supplier.NotFound);
            }

            var availableDeliveryPerson = await _repositoryFacade.GetAvailableDeliveryPersonAsync(order.DeliveryTime);
            if (availableDeliveryPerson == null)
            {
                throw new BussinessArgumentException(ErrorMessages.DeliveryPerson.NoAvailable, 
                    ErrorCodes.DeliveryPerson.NoAvailable);
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

            await _repositoryFacade.AddOrderAsync(orderEntity);

            availableDeliveryPerson.DeliverySlots.Add(order.DeliveryTime);
            await _repositoryFacade.UpdateDeliveryPersonAsync(availableDeliveryPerson);

            _logger.LogInformation(InfoMessages.Order.Added, order.Id);

            return _mapper.Map<Order>(orderEntity);
        }

        public async Task<List<Order>> FilterOrders(string? regionName)
        {
            if(string.IsNullOrEmpty(regionName))
            {
                throw new BussinessArgumentException(ErrorMessages.Region.MustNotBeEmpty, ErrorCodes.Region.MustNotBeEmpty);
            }

            var region = await _repositoryFacade.GetRegionByNameAsync(regionName);
            if (region == null)
            {
                throw new BussinessArgumentException(string.Format(ErrorMessages.Region.NotFound, regionName), 
                    ErrorCodes.Region.NotFound);
            }

            var regionId = region.Id;

            var hasOrders = await _repositoryFacade.HasOrdersAsync(regionId);
            if (!hasOrders)
            {
                throw new BussinessArgumentException(string.Format(ErrorMessages.Order.NoInRegion, regionId),
                    ErrorCodes.Order.NoOrdersFound);
            }

            var firstOrderTime = await _repositoryFacade.GetFirstOrderTimeAsync(regionId);
            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _repositoryFacade.GetOrdersWithinTimeRangeAsync(regionId, firstOrderTime, timeRangeEnd);
            if (filteredOrders == null || filteredOrders.Count == 0)
            {
                throw new BussinessArgumentException(
                    string.Format(ErrorMessages.Order.InTimeRangeNotFound, regionId, firstOrderTime, timeRangeEnd),
                    ErrorCodes.Order.OrdersInTimeRangeNotFound);
            }

            _logger.LogInformation(
                InfoMessages.Order.FoundInRegion, filteredOrders.Count, regionId, firstOrderTime, timeRangeEnd);

            return _mapper.Map<List<Order>>(filteredOrders);
        }

        public async Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending)
        {
            var orders = await _repositoryFacade.GetAllOrdersImmediate();

            OrderSortingHelper.ValidateOrders(orders);

            if (sortBy != null)
            {
                var sortedOrders = OrderSortingHelper.GetSortDelegate(sortBy, descending);

                orders = sortedOrders?.Invoke(orders) ?? orders;
            }

            _logger.LogInformation(InfoMessages.Order.AllOrdersReceived, orders.Count);

            return _mapper.Map<List<Order>>(orders);
        }
    }
}
