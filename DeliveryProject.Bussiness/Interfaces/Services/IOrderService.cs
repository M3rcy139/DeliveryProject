using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> AddOrder(Order order, List<ProductDto> products);
        Task<Order> GetOrderById(Guid id);
        Task UpdateOrder(Order order, List<ProductDto> products);
        Task UpdateOrderStatus(Guid orderId);
        Task DeleteOrder(Guid id);
        Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending);
    }
}
