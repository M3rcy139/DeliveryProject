using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.Core.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using DeliveryProject.Bussiness.Extensions;
using DeliveryProject.Core.Exceptions;
using AutoMapper;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IValidator<Order> _addOrderValidator;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger
            , IValidator<Order> addOrderValidator, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _addOrderValidator = addOrderValidator;
            _mapper = mapper;
        }

        public async Task<int> AddOrder(Order order)
        {
            var (isValid, errors) = await _addOrderValidator.TryValidateAsync(order);

            if (!isValid)
            {
                var errorMessages = errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException("Ошибка валидации", errors);
            }

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
                throw new BussinessArgumentException($"Регион с названием {regionName} не найден.");
            }

            var regionId = region.Id;

            var hasOrders = await _orderRepository.HasOrders(regionId);
            if (!hasOrders)
            {
                throw new BussinessArgumentException($"Заказов в данном районе({regionId}) не найдено");
            }

            var firstOrderTime = await _orderRepository.GetFirstOrderTime(regionId);
            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(regionId, firstOrderTime, timeRangeEnd);
            if (filteredOrders == null || filteredOrders.Count == 0)
            {
                throw new BussinessArgumentException(
                    $"Заказы не найдены для района {regionId} в диапазоне времени с {firstOrderTime} по {timeRangeEnd}");
            }

            _logger.LogInformation(
                $"Найдено {filteredOrders.Count} заказов для района {regionId} в диапазоне от {firstOrderTime} до {timeRangeEnd}");

            return _mapper.Map<List<Order>>(filteredOrders);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();

            if (orders == null || orders.Count == 0)
            {
                throw new BussinessArgumentException("Заказы не найдены");
            }

            _logger.LogInformation($"Получено {orders.Count} заказов");

            return _mapper.Map<List<Order>>(orders);
        }
    }
}
