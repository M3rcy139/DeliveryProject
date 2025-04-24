using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Business.Mediators
{
    public class MediatorHelper<TEntity> where TEntity : class
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public MediatorHelper(
            IOrderRepository orderRepository,
            IInvoiceRepository invoiceRepository,
            IDeliveryPersonRepository deliveryPersonRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _invoiceRepository = invoiceRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task AddEntity(TEntity entity)
        {
            await (typeof(TEntity) switch
            {
                var t when t == typeof(OrderEntity) =>
                    AddOrder((entity as OrderEntity)!),
                var t when t == typeof(InvoiceEntity) =>
                    AddInvoice((entity as InvoiceEntity)!),
                _ => throw new ArgumentException(ErrorMessages.NotSupportedEntityType, typeof(TEntity).Name)
            });
        }
        
        public async Task<TEntity> GetEntityById(Guid id)
        {
            return typeof(TEntity) switch
            {
                var t when t == typeof(CustomerEntity) =>
                    (await GetCustomerById(id) as TEntity)!,
                var t when t == typeof(OrderEntity) =>
                    (await GetOrderById(id) as TEntity)!,
                var t when t == typeof(InvoiceEntity) =>
                    (await GetInvoiceById(id) as TEntity)!,
                _ =>
                    throw new ArgumentException(ErrorMessages.NotSupportedEntityType, typeof(TEntity).Name)
            };
        }

        public async Task DeleteEntityById(Guid id)
        {
            await (typeof(TEntity) switch
            {
                var t when t == typeof(OrderEntity) =>
                    DeleteOrder(id),
                var t when t == typeof(InvoiceEntity) =>
                    DeleteInvoice(id),
                _ => throw new ArgumentException(ErrorMessages.NotSupportedEntityType, typeof(TEntity).Name)
            });
        }
        
        public async Task<List<ProductEntity>> GetProductsByIds(List<Guid> ids)
        {
            var products = await _productRepository.GetProductsById(ids);
            products.ValidateEntities(ErrorMessages.ProductNotFound, ErrorCodes.ProductNotFound);
            return products!;
        }

        public async Task<List<OrderEntity>> GetAllOrders()
        {
            var orders = (await _orderRepository.GetAllOrders()).ToList();
            return orders.IsNullOrEmpty() ? new List<OrderEntity>() : orders;
        }

        public async Task UpdateOrderProducts(OrderEntity order)
        {
            await _orderRepository.UpdateOrderProdutcs(order);
        }

        public async Task UpdateOrderStatus(OrderEntity order)
        {
            await _orderRepository.UpdateOrderStatus(order);
        }

        private async Task AddOrder(OrderEntity orderEntity)
        {
            await _orderRepository.AddOrder(orderEntity);
        }

        private async Task AddInvoice(InvoiceEntity invoiceEntity)
        {
            var availableDeliveryPerson = await GetDeliveryPersonByTime(invoiceEntity);

            invoiceEntity.DeliveryPersonId = availableDeliveryPerson.Id;

            await _invoiceRepository.AddInvoice(invoiceEntity);
            await AddDeliverySlot(availableDeliveryPerson.Id, invoiceEntity.DeliveryTime);
        }

        private async Task<PersonEntity> GetDeliveryPersonByTime(InvoiceEntity invoiceEntity)
        {
            var availableDeliveryPerson = await _deliveryPersonRepository
                .GetDeliveryPersonByTime(invoiceEntity.DeliveryTime);

            availableDeliveryPerson.ValidateEntity(ErrorMessages.NoAvailableDeliveryPersons,
                ErrorCodes.NoAvailableDeliveryPersons);

            return availableDeliveryPerson!;
        }
        
        private async Task AddDeliverySlot(Guid deliveryPersonId, DateTime deliveryTime)
        {
            await _deliveryPersonRepository.AddSlot(new DeliverySlotEntity
            {
                DeliveryPersonId = deliveryPersonId,
                SlotTime = deliveryTime
            });
        }
        
        private async Task<OrderEntity> GetOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

            return order!;
        }

        private async Task<CustomerEntity> GetCustomerById(Guid personId)
        {
            var customer = await _customerRepository.GetCustomerById(personId);
            customer.ValidateEntity(ErrorMessages.CustomerNotFound, ErrorCodes.CustomerNotFound);

            return customer!;
        }

        private async Task<InvoiceEntity> GetInvoiceById(Guid orderId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByOrderId(orderId);
            invoice.ValidateEntity(ErrorMessages.InvoiceNotFound, ErrorCodes.InvoiceNotFound);
            
            return invoice!;
        }

        private async Task DeleteOrder(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.ValidateEntity(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);

            await _orderRepository.DeleteOrder(orderId);
        }

        private async Task DeleteInvoice(Guid orderId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByOrderId(orderId);
            invoice.ValidateEntity(ErrorMessages.InvoiceNotFound, ErrorCodes.InvoiceNotFound);
            await _invoiceRepository.DeleteInvoice(invoice!.Id);
        }
    }
}
