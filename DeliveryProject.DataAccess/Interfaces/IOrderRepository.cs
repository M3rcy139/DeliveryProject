using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(OrderEntity order);
        Task<OrderEntity?> GetOrderById(Guid id);
        Task<List<OrderEntity>> GetOrdersByRegionId(int regionId);
        Task UpdateOrderProducts(OrderEntity orderEntity);
        void UpdateOrderStatus(OrderEntity orderEntity);
        Task RemoveOrder(Guid id);
    }
}
