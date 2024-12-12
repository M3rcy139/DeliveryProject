using DeliveryProject.Core.Interfaces.Services;
using DeliveryProject.Core.Interfaces.Repositories;
using DeliveryProject.Core.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.Application.Services
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

        public async Task AddOrder(Order order)
        {
            var validationResult = await _addOrderValidator.ValidateAsync(order);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogError($"Ошибка валидации: {error.ErrorMessage}");
                }

                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new ValidationException("Ошибка валидации", validationResult.Errors);
            }

            await _orderRepository.AddOrder(order);
            _logger.LogInformation($"Добавлен заказ с ID: {order.Id}");
        }

        public async Task<List<Order>> FilterOrders(int areaId)
        {
            if (areaId <= 0)
            {
                throw new ArgumentException($"Некорректный идентификатор района: {areaId}");
            }

            var firstOrderTime = await _orderRepository.GetFirstOrderTime(areaId);

            var timeRangeEnd = firstOrderTime.AddMinutes(30);

            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(areaId, firstOrderTime, timeRangeEnd);

            _logger.LogInformation(
                $"Найдено {filteredOrders.Count} заказов для района {areaId} в диапазоне от {firstOrderTime} до {timeRangeEnd}");

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
