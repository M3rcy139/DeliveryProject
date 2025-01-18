using DeliveryProject.Bussiness.Helpers;
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

        public async Task<RegionEntity> GetRegionByNameAsync(string regionName)
        {
            OrderServiceHelper.ValidateRegionName(regionName);

            var region = await _orderRepository.GetRegionByName(regionName);
            OrderServiceHelper.ValidateRegion(region, regionName);

            return region;
        }

        public async Task HasOrdersAsync(int regionId)
        {
            var hasOrders = await _orderRepository.HasOrders(regionId);
            OrderServiceHelper.ValidateOrdersInRegion(hasOrders, regionId);
        }

        public async Task<DateTime> GetFirstOrderTimeAsync(int regionId) =>
             await _orderRepository.GetFirstOrderTime(regionId);

        public async Task<List<OrderEntity>> GetOrdersWithinTimeRangeAsync(int regionId, DateTime fromTime, DateTime toTime)
        {
            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(regionId, fromTime, toTime);
            OrderServiceHelper.ValidateOrders(filteredOrders);

            return filteredOrders;
        }

        public async Task<List<OrderEntity>> GetAllOrdersImmediate()
        {
            var orders = await _orderRepository.GetAllOrdersImmediate();
            OrderServiceHelper.ValidateOrders(orders);

            return orders;
        }
    }
}

