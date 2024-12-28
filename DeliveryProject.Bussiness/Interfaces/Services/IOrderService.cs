 using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Interfaces.Services
{
    public interface IOrderService
    {
        Task<int> AddOrder(Order order);
        Task<List<Order>> FilterOrders(string regionName);
        Task<List<Order>> GetAllOrders();
    }
}
