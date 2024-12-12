using DeliveryProject.Core.Models;

namespace DeliveryProject.Core.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<DateTime> GetFirstOrderTime(int areaId);
        Task<List<Order>> GetOrdersWithinTimeRange(int areaId, DateTime fromTime, DateTime toTime);
        Task<List<Order>> GetAllOrders();
    }
}
