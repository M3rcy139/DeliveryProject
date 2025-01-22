using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Dto;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Core.Constants;

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
                RegionId = request.RegionId,
                Weight = request.Weight,
                DeliveryTime = request.DeliveryTime
            };

            var result = await _orderService.AddOrder(order, request.SupplierId);

            return Ok(new
            {
                result,
                message = string.Format(InfoMessages.AddedOrder, order.Id)
            });
        }

        [HttpGet("Orders/Filter")]
        public async Task<IActionResult> FilterOrders(string? regionName)
        {
            var filteredOrders = await _orderService.FilterOrders(regionName);

            return Ok(filteredOrders);
        }

        [HttpGet("Orders/GetAll")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderSortField? sortBy = null,
            [FromQuery] bool descending = false)
        {
            var orders = await _orderService.GetAllOrders(sortBy, descending);

            return Ok(orders);
        }
    }
}
