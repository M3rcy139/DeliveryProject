using Microsoft.AspNetCore.Mvc;
using DeliveryProject.Core.Models;
using DeliveryProject.Application.Dto;
using FluentValidation;
using DeliveryProject.Core.Interfaces.Services;

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

        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {

            try 
            { 
                var order = new Order()
                {
                    Id = Guid.NewGuid(),
                    AreaId = request.AreaId,
                    Weight = request.Weight,
                    DeliveryTime = request.DeliveryTime
                };

                await _orderService.AddOrder(order);

                return Ok(order);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = "Ошибка валидации",
                    details = ex.Errors.Select(e => e.ErrorMessage)
                });
            }
        }

        [HttpGet("filter-orders")]
        public async Task<IActionResult> FilterOrders(int areaId)
        {
            var filteredOrders = await _orderService.FilterOrders(areaId);

            return Ok(filteredOrders);
        }

        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();

            return Ok(orders);
        }
    }
}
