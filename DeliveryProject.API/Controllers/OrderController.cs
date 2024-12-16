using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Core.Models;
using DeliveryProject.API.Dto;
using FluentValidation;
using DeliveryProject.Bussiness.Contract.Interfaces.Services;
using DeliveryProject.API.Attributes;

namespace DeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Order/Add")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                RegionId = request.regionId,
                Weight = request.Weight,
                DeliveryTime = request.DeliveryTime
            };

            var affectedRows = await _orderService.AddOrder(order);

            return Ok(new
            {
                order,
                affectedRows,
                message = "Заказ успешно добавлен"
            });
        }

        [HttpGet("Orders/Filter")]
        public async Task<IActionResult> FilterOrders(string regionName)
        {
            var filteredOrders = await _orderService.FilterOrders(regionName);

            return Ok(filteredOrders);
        }

        [HttpGet("Orders/GetAll")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();

            return Ok(orders);
        }
    }
}
