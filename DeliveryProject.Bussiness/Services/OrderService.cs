using DeliveryProject.Bussiness.Contract.Interfaces.Services;
using DeliveryProject.Repositories.Interfaces;
using DeliveryProject.Core.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using DeliveryProject.Bussiness.Extensions;

namespace DeliveryProject.Bussiness.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IValidator<Order> _addOrderValidator;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger
            , IValidator<Order> addOrderValidator)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _addOrderValidator = addOrderValidator;
        }

        public async Task<int> AddOrder(Order order)
        {
            var (isValid, errors) = await _addOrderValidator.TryValidateAsync(order);

            if (!isValid)
            {
                var errorMessages = errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException("Ошибка валидации", errors);
            }

            var result = await _orderRepository.AddOrder(order);

            _logger.LogInformation($"Добавлен заказ с ID: {order.Id}");

            return result;
        }

        public async Task<List<Order>> FilterOrders(string regionName)
        {
            var region = await _orderRepository.GetRegionByName(regionName);

            var regionId = region.Id;


            var firstOrderTime = await _orderRepository.GetFirstOrderTime(regionId);

            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(regionId, firstOrderTime, timeRangeEnd);

            _logger.LogInformation(
                $"Найдено {filteredOrders.Count} заказов для района {regionId} в диапазоне от {firstOrderTime} до {timeRangeEnd}");

            return filteredOrders;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();

            _logger.LogInformation($"Получено {orders.Count} заказов");

            return orders;
        }
    }
}
