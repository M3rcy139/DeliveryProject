using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Bussiness.Facades
{
    public class OrderRepositoryFacade
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;

        public OrderRepositoryFacade(
            IOrderRepository orderRepository,
            ISupplierRepository supplierRepository,
            IDeliveryPersonRepository deliveryPersonRepository)
        {
            _orderRepository = orderRepository;
            _supplierRepository = supplierRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
        }

        public Task AddOrderAsync(OrderEntity orderEntity) => _orderRepository.AddOrder(orderEntity);

        public Task<RegionEntity> GetRegionByNameAsync(string regionName) =>
            _orderRepository.GetRegionByName(regionName);

        public Task<bool> HasOrdersAsync(int regionId) => _orderRepository.HasOrders(regionId);

        public Task<DateTime> GetFirstOrderTimeAsync(int regionId) => _orderRepository.GetFirstOrderTime(regionId);

        public Task<List<OrderEntity>> GetOrdersWithinTimeRangeAsync(int regionId, DateTime fromTime, DateTime toTime) =>
            _orderRepository.GetOrdersWithinTimeRange(regionId, fromTime, toTime);

        public Task<SupplierEntity?> GetSupplierByIdAsync(int supplierId) =>
            _supplierRepository.GetByIdAsync(supplierId);

        public Task<DeliveryPersonEntity?> GetAvailableDeliveryPersonAsync(DateTime deliveryTime) =>
            _deliveryPersonRepository.GetAvailableDeliveryPersonAsync(deliveryTime);

        public Task UpdateDeliveryPersonAsync(DeliveryPersonEntity deliveryPerson) =>
            _deliveryPersonRepository.UpdateAsync(deliveryPerson);

        public Task<List<OrderEntity>> GetAllOrdersImmediate() =>
            _orderRepository.GetAllOrdersImmediate();
    }
}

