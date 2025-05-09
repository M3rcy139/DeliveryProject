using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Business.DomainServices;

public class OrderDomainService
{
    private readonly IOrderRepository _orderRepository;

    public OrderDomainService(IOrderRepository orderRepository)
        => _orderRepository = orderRepository;

    public async Task AddOrder(OrderEntity orderEntity)
    {
        await _orderRepository.AddOrder(orderEntity);
    }

    public async Task<OrderEntity> GetOrderById(Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);
        
        return order!;
    }
    
    public async Task<List<OrderEntity>> GetOrdersByRegionId(int regionId)
    {
        var orders = (await _orderRepository.GetOrdersByRegionId(regionId)).ToList();
        
        return orders.IsNullOrEmpty() ? new List<OrderEntity>() : orders;
    }
    
    public async Task UpdateOrderProducts(OrderEntity order)
    {
        await _orderRepository.UpdateOrderProducts(order);
    }
    
    public void UpdateOrderStatus(OrderEntity order)
    {
        _orderRepository.UpdateOrderStatus(order);
    }
    
    public async Task RemoveOrder(Guid orderId)
    {
        await GetOrderById(orderId);

        await _orderRepository.RemoveOrder(orderId);
    }
}