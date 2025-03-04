using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> AddOrder(Order order, List<ProductItemViewModel> products);
        Task<Order> GetOrderById(Guid id);
        Task UpdateOrder(Order order, List<ProductItemViewModel> products);
        Task DeleteOrder(Guid id);
        Task<List<Order>> FilterOrders(string? regionName);
        Task<List<Order>> GetAllOrders(OrderSortField? sortBy, bool descending);
    }
}
