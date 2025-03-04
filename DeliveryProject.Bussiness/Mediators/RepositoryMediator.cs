using DeliveryProject.Bussiness.Helpers;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Extensions;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.Bussiness.Mediators
{
    public class RepositoryMediator
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;

        public RepositoryMediator(
            IOrderRepository orderRepository,
            ISupplierRepository supplierRepository,
            IDeliveryPersonRepository deliveryPersonRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _supplierRepository = supplierRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderEntity> AddOrderAsync(OrderEntity orderEntity)
        {
            var suppliers = await GetAndValidateSuppliers(orderEntity);

            orderEntity.Persons.AddRange(suppliers);

            var availableDeliveryPerson = await GetAndValidateDeliveryPerson(orderEntity); 

            orderEntity.Persons.Add(availableDeliveryPerson);

            await _orderRepository.AddOrder(orderEntity);

            AddDeliverySlot(availableDeliveryPerson.Id, orderEntity.DeliveryTime);

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

            orderEntity.Persons.Add(order.Persons
                .First(p => p.Role.Role == RoleType.DeliveryPerson));

            orderEntity.Persons.Add(order.Persons
                .First(p => p.Role.Role == RoleType.Customer));

            var suppliers = await GetAndValidateSuppliers(orderEntity);

            orderEntity.Persons.AddRange(suppliers);

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

        public async Task<CustomerEntity?> GetAndValidateCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            customer.ValidateEntity(ErrorMessages.CustomerNotFound, ErrorCodes.CustomerNotFound);

            return customer;
        }

        public async Task<List<ProductEntity?>> GetAndValidateProductsAsync(List<Guid> productIds)
        {
            var products = await _productRepository.GetProductsByIdAsync(productIds);

            products.ForEach(product =>
            {
                product.ValidateEntity(ErrorMessages.ProductNotFound, ErrorCodes.ProductNotFound);
            });

            return products;
        }

        private async Task<List<SupplierEntity?>> GetAndValidateSuppliers(OrderEntity orderEntity)
        {
            var suppliers = await _supplierRepository.GetSuppliersByProductIdsAsync(
                orderEntity.OrderProducts.Select(op => op.ProductId).ToList()
            );

            suppliers.ForEach(supplier =>
            {
                supplier.ValidateEntity(ErrorMessages.SupplierNotFound, ErrorCodes.SupplierNotFound);
            });

            return suppliers;
        }

        private async Task<OrderEntity?> GetAndValidateOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

            return order;
        }

        private async Task<DeliveryPersonEntity?> GetAndValidateDeliveryPerson(OrderEntity orderEntity)
        {
            var availableDeliveryPerson = await _deliveryPersonRepository
                .GetAvailableDeliveryPersonAsync(orderEntity.DeliveryTime);

            availableDeliveryPerson.ValidateEntity(ErrorMessages.NoAvailableDeliveryPersons,
                    ErrorCodes.NoAvailableDeliveryPersons);

            return availableDeliveryPerson;
        }

        private async void AddDeliverySlot(Guid deliveryPersonId, DateTime deliveryTime)
        {
            await _deliveryPersonRepository.AddSlotAsync(new DeliverySlotEntity
            {
                DeliveryPersonId = deliveryPersonId,
                SlotTime = deliveryTime
            });
        }
    }
}

