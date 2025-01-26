using DeliveryProject.Bussiness.Helpers;
using System.Collections.Concurrent;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Bussiness.Mediators
{
    public class RepositoryMediator
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;

        public RepositoryMediator(
            IOrderRepository orderRepository,
            ISupplierRepository supplierRepository,
            IDeliveryPersonRepository deliveryPersonRepository)
        {
            _orderRepository = orderRepository;
            _supplierRepository = supplierRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
        }

        public async Task<OrderEntity> AddOrderAsync(OrderEntity orderEntity)
        {
            var supplier = await _supplierRepository.GetByIdAsync(orderEntity.SupplierId);
            OrderServiceHelper.ValidateSupplier(supplier);

            var availableDeliveryPerson = await _deliveryPersonRepository.GetAvailableDeliveryPersonAsync(orderEntity.DeliveryTime);
            OrderServiceHelper.ValidateDeliveryPerson(availableDeliveryPerson);

            orderEntity.DeliveryPersonId = availableDeliveryPerson.Id;
            await _orderRepository.AddOrder(orderEntity);

            availableDeliveryPerson.DeliverySlots.Add(orderEntity.DeliveryTime);
            await _deliveryPersonRepository.UpdateAsync(availableDeliveryPerson);

            return orderEntity;
        }

        public async Task<OrderEntity?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            OrderServiceHelper.ValidateOrder(order);

            return order;
        }

        public async Task UpdateOrderAsync(OrderEntity orderEntity)
        {
            var order = await _orderRepository.GetOrderById(orderEntity.Id);
            OrderServiceHelper.ValidateOrder(order);

            var supplier = await _supplierRepository.GetByIdAsync(orderEntity.SupplierId);
            OrderServiceHelper.ValidateSupplier(supplier);

            orderEntity.DeliveryPersonId = order.DeliveryPersonId;

            await _orderRepository.UpdateOrder(orderEntity); 
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            OrderServiceHelper.ValidateOrder(order);

            await _orderRepository.DeleteOrder(orderId); 
        }

        public async Task<RegionEntity> GetRegionByNameAsync(string regionName)
        {
            OrderServiceHelper.ValidateRegionName(regionName);

            var region = await _orderRepository.GetRegionByName(regionName);
            OrderServiceHelper.ValidateRegion(region, regionName);

            return region;
        }

        public async Task<DateTime> GetFirstOrderTimeAsync(int regionId)
        {
            var hasOrders = await _orderRepository.HasOrders(regionId);
            OrderServiceHelper.ValidateOrdersInRegion(hasOrders, regionId);

            return await _orderRepository.GetFirstOrderTime(regionId);

        }

        public async Task<List<OrderEntity>> GetOrdersWithinTimeRangeAsync(int regionId, DateTime fromTime, DateTime toTime)
        {
            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(regionId, fromTime, toTime);
            OrderServiceHelper.ValidateOrders(ref filteredOrders);

            return filteredOrders;
        }

        public async Task<List<OrderEntity>> GetAllOrdersImmediate()
        {
            var concurrentOrders = await _orderRepository.GetAllOrdersImmediate();
            var orders = concurrentOrders.ToList();
            OrderServiceHelper.ValidateOrders(ref orders);

            return orders;
        }
    }
}

