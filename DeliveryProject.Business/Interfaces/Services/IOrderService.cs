using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Business.Interfaces.Services
{
    public interface IOrderService
    {
        Task CreateOrder(Order order, List<ProductDto> products);
        Task<Order> GetOrderById(Guid id);
        Task<List<Order>> GetOrdersByRegionId(int regionId, OrderSortField? sortBy, bool descending);
        Task UpdateOrderProducts(Order order, List<ProductDto> products);
        Task UpdateOrderStatus(Guid orderId, OrderStatus status);
        Task RemoveOrder(Guid id);
    }
}
