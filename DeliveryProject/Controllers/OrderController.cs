using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Persistence;
using DeliveryProject.Core.Models;
using DeliveryProject.Persistence.Interfaces;
using DeliveryProject.Application.Contracts;
using FluentValidation;

namespace DeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DeliveryDbContext _context;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IValidator<AddOrderRequest> _addOrderValidator;

        public OrderController(DeliveryDbContext context, ILogger<OrderController> logger, 
            IOrderRepository orderRepository, IValidator<AddOrderRequest> addOrderValidator)
        {
            _context = context;
            _logger = logger;
            _orderRepository = orderRepository;
            _addOrderValidator = addOrderValidator;
        }

        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            try
            {
                var validationResult = await _addOrderValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.LogError("Ошибка валидации: {ErrorMessage}", error.ErrorMessage);
                    }

                    return BadRequest(validationResult.Errors);
                }

                var order = new Order()
                {
                    Id = Guid.NewGuid(),
                    AreaId = request.AreaId,
                    Weight = request.Weight,
                    DeliveryTime = request.DeliveryTime
                };

                await _orderRepository.AddOrder(order);

                _logger.LogInformation("Добавлен заказ с ID: {OrderId}", order.Id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("filter-orders")]
        public async Task<IActionResult> FilterOrders(int areaId)
        {
            try
            {
                if (areaId <= 0)
                {
                    _logger.LogError("Некорректные данные заказа");
                    return BadRequest("Некорректные данные заказа");
                }

                var firstOrderTime = await _orderRepository.GetFirstOrderTime(areaId);

                var timeRangeEnd = firstOrderTime.AddMinutes(30);

                var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(areaId, firstOrderTime, timeRangeEnd);

                _logger.LogInformation("Найдено {OrderCount} заказов для района {AreaId} в диапазоне от {FirstOrderTime} до {TimeRangeEnd}",
                    filteredOrders.Count, areaId, firstOrderTime, timeRangeEnd);

                return Ok(filteredOrders);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrders();
                _logger.LogInformation("Получено {OrderCount} заказов", orders.Count);
                return Ok(orders);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
