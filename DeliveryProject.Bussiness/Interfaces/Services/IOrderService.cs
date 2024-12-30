using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
    public interface IOrderService
    {
        Task<int> AddOrder(Order order);
        Task<List<Order>> FilterOrders(string regionName);
        Task<List<Order>> GetAllOrders(SortField? sortBy, bool descending);
    }
}
