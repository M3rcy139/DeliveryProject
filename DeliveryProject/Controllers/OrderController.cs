using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeliveryProject.Persistence;
using DeliveryProject.Core.Models;
using DeliveryProject.Persistence.Interfaces;
using DeliveryProject.Application.Contracts;

namespace DeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DeliveryDbContext _context;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderController(DeliveryDbContext context, ILogger<OrderController> logger, IOrderRepository orderRepository)
        {
            _context = context;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        // Добавление нового заказа
        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            try
            {
                if (request == null || request.Weight <= 0 || request.AreaId <= 0)
                {
                    _logger.LogError("Некорректные данные заказа");
                    return BadRequest("Некорректные данные заказа");
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
                var firstOrderTime = await _orderRepository.GetFirstOrderTime(areaId);
                //if (firstOrderTime == null)
                //{
                //    _logger.LogWarning("Нет заказов для района с ID {AreaId}", areaId);
                //    return NotFound("Заказы для данного района не найдены.");
                //}

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

        // Получение всех заказов
        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrders();
                _logger.LogInformation("Получено {OrderCount} заказов", orders.Count);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
