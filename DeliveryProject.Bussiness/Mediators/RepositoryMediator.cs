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
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public RepositoryMediator(
            IOrderRepository orderRepository,
            IDeliveryRepository deliveryRepository,
            IDeliveryPersonRepository deliveryPersonRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _deliveryRepository = deliveryRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderEntity> AddOrder(OrderEntity orderEntity)
        {
            await _orderRepository.AddOrder(orderEntity);
            
            return orderEntity;
        }

        public async Task<OrderEntity> GetOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

            return order!;
        }

        public async Task UpdateOrder(Guid orderId, List<OrderProductEntity> orderProducts, decimal amount)
        {
            var order = await GetOrderById(orderId);

            order.Amount = amount;

            order.OrderProducts.Clear();
            order.OrderProducts.AddRange(orderProducts
                .Select(op => new OrderProductEntity { ProductId = op.ProductId, OrderId = op.OrderId, Quantity = op.Quantity }));

            await _orderRepository.UpdateOrder(order);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

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

            return customer!;
        }

        public async Task<List<ProductEntity>> GetProductsByIds(List<Guid> productIds)
        {
            var products = await _productRepository.GetProductsById(productIds);
            products.ValidateEntities(ErrorMessages.ProductNotFound, ErrorCodes.ProductNotFound);
            return products!;
        }

        public async Task AddInvoice(InvoiceEntity invoiceEntity)
        {
            var availableDeliveryPerson = await GetDeliveryPersonByTime(invoiceEntity);

            invoiceEntity.DeliveryPersonId = availableDeliveryPerson.Id;

            await _deliveryRepository.AddInvoice(invoiceEntity);

            AddDeliverySlot(availableDeliveryPerson.Id, invoiceEntity.DeliveryTime);
        }
        
        public async Task<InvoiceEntity> GetInvoiceByOrderId(Guid orderId)
        {
            var invoice = await _deliveryRepository.GetInvoiceByOrderId(orderId);
            invoice.ValidateEntity(ErrorMessages.InvoiceNotFound, ErrorCodes.InvoiceNotFound);

            return invoice!;
        }

        public async Task DeleteInvoice(Guid orderId)
        {
            var invoice  = await GetInvoiceByOrderId(orderId);

            await _deliveryRepository.DeleteInvoice(invoice.Id); 
        }
        
        private async Task<PersonEntity> GetDeliveryPersonByTime(InvoiceEntity invoiceEntity)
        {
            var availableDeliveryPerson = await _deliveryPersonRepository
                .GetDeliveryPersonByTime(invoiceEntity.DeliveryTime);

            availableDeliveryPerson.ValidateEntity(ErrorMessages.NoAvailableDeliveryPersons,
                    ErrorCodes.NoAvailableDeliveryPersons);

            return availableDeliveryPerson!;
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

