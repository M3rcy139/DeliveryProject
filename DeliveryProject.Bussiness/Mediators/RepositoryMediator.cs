using DeliveryProject.Bussiness.Helpers;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Extensions;

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
            await GetAndValidateSupplierById(orderEntity.SupplierId);

            var availableDeliveryPerson = await _deliveryPersonRepository.GetAvailableDeliveryPersonAsync(orderEntity.DeliveryTime);
            availableDeliveryPerson.ValidateEntity(ErrorMessages.NoAvailableDeliveryPersons,
                    ErrorCodes.NoAvailableDeliveryPersons);

            orderEntity.DeliveryPersonId = availableDeliveryPerson.Id;
            await _orderRepository.AddOrder(orderEntity);

            availableDeliveryPerson.DeliverySlots.Add(orderEntity.DeliveryTime);
            await _deliveryPersonRepository.UpdateAsync(availableDeliveryPerson);

            return orderEntity;
        }

        public async Task<OrderEntity?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await GetAndValidateOrderById(orderId);

            return order;
        }

        public async Task UpdateOrderAsync(OrderEntity orderEntity)
        {
            var order = await GetAndValidateOrderById(orderEntity.Id);

            await GetAndValidateSupplierById(orderEntity.SupplierId);

            orderEntity.DeliveryPersonId = order.DeliveryPersonId;

            await _orderRepository.UpdateOrder(orderEntity); 
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);

            await _orderRepository.DeleteOrder(orderId); 
        }

        public async Task<RegionEntity> GetRegionByNameAsync(string regionName)
        {
            regionName.ValidateNotEmpty(ErrorMessages.RegionMustNotBeEmpty, ErrorCodes.MustNotBeEmpty);

            var region = await _orderRepository.GetRegionByName(regionName);
            region.ValidateEntity(ErrorMessages.RegionNotFound, ErrorCodes.RegionNotFound);

            return region;
        }

        public async Task<DateTime> GetFirstOrderTimeAsync(int regionId)
        {
            var hasOrders = await _orderRepository.HasOrders(regionId);
            ValidationHelper.ValidateOrdersInRegion(hasOrders, regionId);

            return await _orderRepository.GetFirstOrderTime(regionId);

        }

        public async Task<List<OrderEntity>> GetOrdersWithinTimeRangeAsync(int regionId, DateTime fromTime, DateTime toTime)
        {
            var filteredOrders = await _orderRepository.GetOrdersWithinTimeRange(regionId, fromTime, toTime);

            return filteredOrders.IsNullOrEmpty() ? new List<OrderEntity>() : filteredOrders;
        }

        public async Task<List<OrderEntity>> GetAllOrdersImmediate()
        {
            var concurrentOrders = await _orderRepository.GetAllOrdersImmediate();
            var orders = concurrentOrders.ToList();

            return orders.IsNullOrEmpty() ? new List<OrderEntity>() : orders;
        }

        private async Task GetAndValidateSupplierById(int supplierId)
        {
            await _supplierRepository.GetByIdAsync(supplierId)
                .ContinueWith(async t =>
                   (await t).ValidateEntity(ErrorMessages.SupplierNotFound, ErrorCodes.SupplierNotFound));
        }

        private async Task<OrderEntity?> GetAndValidateOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

            return order;
        }
    }
}

