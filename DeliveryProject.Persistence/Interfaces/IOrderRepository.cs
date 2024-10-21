using DeliveryProject.Core.Models;

namespace DeliveryProject.Persistence.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<List<Order>> FilterOrder(int areaId, DateTime fromTime, DateTime toTime);
        Task<List<Order>> GetAllOrders();
    }
}
