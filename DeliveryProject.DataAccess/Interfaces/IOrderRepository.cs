using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(OrderEntity order);
        Task<OrderEntity?> GetOrderById(Guid id);
        Task UpdateOrderProdutcs(OrderEntity orderEntity);
        Task UpdateOrderStatus(OrderEntity orderEntity);
        Task DeleteOrder(Guid id);
        Task<List<OrderEntity>> GetAllOrders();
    }
}
