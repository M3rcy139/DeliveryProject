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
        Task<List<OrderEntity>> GetAllOrdersImmediate();
    }
}
