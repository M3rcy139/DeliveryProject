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
        private readonly IPersonRepository _personRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;

        public RepositoryMediator(
            IOrderRepository orderRepository,
            IPersonRepository personRepository,
            IDeliveryPersonRepository deliveryPersonRepository,
            ICustomerRepository customerRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _personRepository = personRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _customerRepository = customerRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderEntity> AddOrderAsync(OrderEntity orderEntity, decimal amount)
        {
            var suppliers = await GetAndValidateSuppliers(orderEntity);
            var availableDeliveryPerson = await GetAndValidateDeliveryPerson(orderEntity);

            orderEntity.OrderPersons.AddRange(suppliers.Select(s => new OrderPersonEntity { PersonId = s.Id }));
            orderEntity.OrderPersons.Add(new OrderPersonEntity { PersonId = availableDeliveryPerson.Id });

            var deliveryTime = await CalculateDeliveryTime();

            var invoice = new InvoiceEntity
            {
                Id = Guid.NewGuid(),
                OrderId = orderEntity.Id,
                DeliveryPersonId = availableDeliveryPerson.Id,
                Amount = amount,
                DeliveryTime = deliveryTime,
                IsExecuted = false
            };

            orderEntity.Invoice = invoice;

            await _orderRepository.AddOrder(orderEntity);

            AddDeliverySlot(availableDeliveryPerson.Id, deliveryTime);

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

            order.Status = orderEntity.Status;

            //orderEntity.Persons.Add(order.Persons
            //    .First(p => p.Role.Role == RoleType.DeliveryPerson));

            //orderEntity.Persons.Add(order.Persons
            //    .First(p => p.Role.Role == RoleType.Customer));

            var suppliers = await GetAndValidateSuppliers(orderEntity);

            order.OrderPersons.AddRange(suppliers.Select(s => new OrderPersonEntity { PersonId = s.Id }));

            await _orderRepository.UpdateOrder(order); 
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);

            await _orderRepository.DeleteOrder(orderId); 
        }

        public async Task<List<OrderEntity>> GetAllOrdersImmediate()
        {
            var concurrentOrders = await _orderRepository.GetAllOrdersImmediate();
            var orders = concurrentOrders.ToList();

            return orders.IsNullOrEmpty() ? new List<OrderEntity>() : orders;
        }

        public async Task<PersonEntity?> GetAndValidateCustomerAsync(Guid personId)
        {
            var customer = await _customerRepository.GetCustomerByIdAndRoleAsync(personId);
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

        private async Task<List<PersonEntity>> GetAndValidateSuppliers(OrderEntity orderEntity)
        {
            var suppliers = await _supplierRepository.GetPersonsByProductIdsAndRoleAsync(
                orderEntity.OrderProducts.Select(op => op.ProductId).ToList());

            suppliers.ForEach(supplier =>
            {
                supplier.ValidateEntity(ErrorMessages.SupplierNotFound, ErrorCodes.SupplierNotFound);
            });

            return suppliers;
        }

        private async Task<OrderEntity> GetAndValidateOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

            return order;
        }

        private async Task<PersonEntity> GetAndValidateDeliveryPerson(OrderEntity orderEntity)
        {
            var availableDeliveryPerson = await _deliveryPersonRepository
                .GetAvailableDeliveryPersonAsync(orderEntity.Invoice.DeliveryTime);

            availableDeliveryPerson.ValidateEntity(ErrorMessages.NoAvailableDeliveryPersons,
                    ErrorCodes.NoAvailableDeliveryPersons);

            return availableDeliveryPerson;
        }

        private async Task<DateTime> CalculateDeliveryTime()
        {
            var random = new Random();

            return new DateTime(2027, 5, 5, random.Next(0, 24), random.Next(0, 60), 0);
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

