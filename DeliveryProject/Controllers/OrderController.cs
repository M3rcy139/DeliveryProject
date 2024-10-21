using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeliveryProject.Persistence;
using DeliveryProject.Core.Models;
using DeliveryProject.Persistence.Interfaces;

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
        [HttpPost("add-order/{order}")]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            if (order == null || order.Weight <= 0 || order.AreaId <= 0)
            {
                _logger.LogError("Некорректные данные заказа");
                return BadRequest("Некорректные данные заказа");
            }

            await _orderRepository.AddOrder(order);

            _logger.LogInformation("Добавлен заказ с ID: {OrderId}", order.Id);
            return Ok(order);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterOrders(int areaId, DateTime fromTime, DateTime toTime)
        {
            if (toTime <= fromTime)
            {
                _logger.LogError("Некорректный диапазон времени: {FromTime} - {ToTime}", fromTime, toTime);
                return BadRequest("Некорректный диапазон времени.");
            }

            var filteredOrders = await _orderRepository.FilterOrder(areaId, fromTime, toTime);

            _logger.LogInformation("Отфильтровано {OrderCount} заказов для района {AreaId} в диапазоне {FromTime} - {ToTime}", filteredOrders.Count, areaId, fromTime, toTime);

            return Ok(filteredOrders);
        }

        // Получение всех заказов
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            _logger.LogInformation("Получено {OrderCount} заказов", orders.Count);
            return Ok(orders);
        }
    }
}
