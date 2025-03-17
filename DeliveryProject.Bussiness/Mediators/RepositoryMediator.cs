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
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public RepositoryMediator(
            IOrderRepository orderRepository,
            IDeliveryPersonRepository deliveryPersonRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderEntity> AddOrder(OrderEntity orderEntity)
        {
            var availableDeliveryPerson = await GetDeliveryPersonByTime(orderEntity);
            orderEntity.OrderPersons.Add(new OrderPersonEntity { Person = availableDeliveryPerson });

            orderEntity.Invoice.DeliveryPersonId = availableDeliveryPerson.Id;

            await _orderRepository.AddOrder(orderEntity);

            AddDeliverySlot(availableDeliveryPerson.Id, orderEntity.Invoice.DeliveryTime);

            return orderEntity;
        }

        public async Task<OrderEntity> GetOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

            return order;
        }

        public async Task UpdateOrder(Guid orderId, List<OrderProductEntity> orderProducts, decimal amount)
        {
            var order = await GetOrderById(orderId);

            order.Invoice.Amount = amount;

            var customer = order.OrderPersons.First(p => p.Person.Role.RoleType == RoleType.Customer);
            var deliveryPerson = order.OrderPersons.First(p => p.Person.Role.RoleType == RoleType.DeliveryPerson);

            order.OrderPersons.Clear();
            order.OrderPersons.Add(customer);
            order.OrderPersons.Add(deliveryPerson);

            order.OrderProducts.Clear();
            order.OrderProducts.AddRange(orderProducts
                .Select(op => new OrderProductEntity { ProductId = op.ProductId, OrderId = op.OrderId, Quantity = op.Quantity }));

            await _orderRepository.UpdateOrder(order);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            var order = await GetOrderById(orderId);

            await _orderRepository.DeleteOrder(orderId); 
        }

        public async Task<List<OrderEntity>> GetAllOrders()
        {
            var concurrentOrders = await _orderRepository.GetAllOrders();
            var orders = concurrentOrders.ToList();

            return orders.IsNullOrEmpty() ? new List<OrderEntity>() : orders;
        }

        public async Task<CustomerEntity> GetCustomerById(Guid personId)
        {
            var customer = await _customerRepository.GetCustomerById(personId);
            customer.ValidateEntity(ErrorMessages.CustomerNotFound, ErrorCodes.CustomerNotFound);

            return customer;
        }

        public async Task<List<ProductEntity>> GetProductsByIds(List<Guid> productIds)
        {
            var products = await _productRepository.GetProductsById(productIds);
            products.ValidateEntities(ErrorMessages.ProductNotFound, ErrorCodes.ProductNotFound);
            return products;
        }

        private async Task<PersonEntity> GetDeliveryPersonByTime(OrderEntity orderEntity)
        {
            var availableDeliveryPerson = await _deliveryPersonRepository
                .GetDeliveryPersonByTime(orderEntity.Invoice.DeliveryTime);

            availableDeliveryPerson.ValidateEntity(ErrorMessages.NoAvailableDeliveryPersons,
                    ErrorCodes.NoAvailableDeliveryPersons);

            return availableDeliveryPerson;
        }

        public DateTime CalculateDeliveryTime()
        {
            var random = new Random();

            return new DateTime(2027, 5, 5, random.Next(0, 24), random.Next(0, 60), 0);
        }

        private async void AddDeliverySlot(Guid deliveryPersonId, DateTime deliveryTime)
        {
            await _deliveryPersonRepository.AddSlot(new DeliverySlotEntity
            {
                DeliveryPersonId = deliveryPersonId,
                SlotTime = deliveryTime
            });
        }
    }
}

