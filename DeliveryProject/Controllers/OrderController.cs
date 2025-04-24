using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Dto;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Constants.InfoMessages;

namespace DeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IDeliveryService _deliveryService;

        public OrderController(IOrderService orderService, IDeliveryService deliveryService)
        {
            _orderService = orderService;
            _deliveryService = deliveryService;
        }

        [HttpPost("Order/Add")]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest model)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                OrderPersons = new List<OrderPerson> 
                {
                    new OrderPerson { PersonId = model.CustomerId }
                },
            };

            var result = await _orderService.AddOrder(order, model.Products);

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
        public async Task<IActionResult> UpdateOrder([FromBody] OrderRequest model)
        {
            var order = new Order()
            {
                Id = model.OrderId.Value,
            };

            await _orderService.UpdateOrderProducts(order, model.Products);

            return Ok(new { message = string.Format(InfoMessages.UpdatedOrder, order.Id) });
        }

        [HttpDelete("Order/Delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            await _deliveryService.DeleteInvoice(orderId);
            await _orderService.DeleteOrder(orderId);
            
            return Ok(new { message = string.Format(InfoMessages.DeletedOrder, orderId) });
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
