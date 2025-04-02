using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IOrderService _orderService;

        public DeliveryController(IDeliveryService deliveryService, IOrderService orderService)
        {
            _deliveryService = deliveryService;
            _orderService = orderService;
        }
        
        [HttpPost("Invoice/Add")]
        public async Task<IActionResult> AddOrder(Guid orderId)
        {
            await _deliveryService.AddInvoice(orderId);
            await _orderService.UpdateOrderStatus(orderId);

            return Ok(new
            {
                message = string.Format(InfoMessages.AddedInvoice, orderId),
            });
        }
    }
}
