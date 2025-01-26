using DeliveryProject.DataAccess.Entities;
using System.Collections.Concurrent;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(OrderEntity order);
        Task<OrderEntity> GetOrderById(Guid id);
        Task UpdateOrder(OrderEntity orderEntity);
        Task DeleteOrder(Guid id);
        Task<RegionEntity> GetRegionByName(string regionName);
        Task<bool> HasOrders(int regionId);
        Task<DateTime> GetFirstOrderTime(int regionId);
        Task<List<OrderEntity>> GetOrdersWithinTimeRange(int regionId, DateTime fromTime, DateTime toTime);
        Task<ConcurrentBag<OrderEntity>> GetAllOrdersImmediate();
    }
}
