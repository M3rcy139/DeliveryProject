using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(OrderEntity order);
        Task<RegionEntity> GetRegionByName(string regionName);
        Task<bool> HasOrders(int regionId);
        Task<DateTime> GetFirstOrderTime(int regionId);
        Task<List<OrderEntity>> GetOrdersWithinTimeRange(int regionId, DateTime fromTime, DateTime toTime);
        Task<List<OrderEntity>> GetAllOrdersImmediate();
    }
}
