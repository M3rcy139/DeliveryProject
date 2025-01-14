using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> AddOrder(Order order, int supplierId);
        Task<List<Order>> FilterOrders(string? regionName);
        Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending);
    }
}
