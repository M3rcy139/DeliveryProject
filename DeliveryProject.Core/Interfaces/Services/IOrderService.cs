using DeliveryProject.Core.Models;

namespace DeliveryProject.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task AddOrder(Order order);
        Task<List<Order>> FilterOrders(int areaId);
        Task<List<Order>> GetAllOrders();
    }
}
