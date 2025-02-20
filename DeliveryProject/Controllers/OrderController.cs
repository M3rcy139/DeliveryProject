using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Dto;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Constants;
using System.Diagnostics;

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
        public async Task<IActionResult> AddOrder([FromBody] OrderViewModel model)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                RegionId = model.RegionId,
                Weight = model.Weight,
                DeliveryTime = model.DeliveryTime,
                SupplierId = model.SupplierId
            };

            var result = await _orderService.AddOrder(order);

            return Ok(new
            {
                result,
                message = string.Format(InfoMessages.AddedOrder, order.Id),
            });
        }

        [HttpGet("Order/{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            return Ok(order);
        }

        [HttpPut("Order/Update")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderViewModel model)
        {
            var order = new Order()
            {
                Id = model.OrderId.Value,
                RegionId = model.RegionId,
                Weight = model.Weight,
                DeliveryTime = model.DeliveryTime,
                SupplierId = model.SupplierId
            };

            await _orderService.UpdateOrder(order);

            return Ok(new { message = string.Format(InfoMessages.UpdatedOrder, order.Id) });
        }

        [HttpDelete("Order/Delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            await _orderService.DeleteOrder(orderId);

            return Ok(new { message = string.Format(InfoMessages.DeletedOrder, orderId) });
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
