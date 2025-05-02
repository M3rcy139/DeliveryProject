using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Business.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> AddOrder(Order order, List<ProductDto> products);
        Task<Order> GetOrderById(Guid id);
        Task UpdateOrderProducts(Order order, List<ProductDto> products);
        Task UpdateOrderStatus(Guid orderId, OrderStatus status);
        Task RemoveOrder(Guid id);
        Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending);
    }
}
