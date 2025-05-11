using DeliveryProject.Business.DomainServices;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Mediators
{
    public class Mediator<TEntity> where TEntity : class
    {
        private readonly OrderDomainService _orderDomainService;
        private readonly CustomerDomainService _customerDomainService;
        private readonly ProductDomainService _productDomainService;
        private readonly InvoiceDomainService _invoiceDomainService;
        private readonly DeliveryPersonDomainService _deliveryPersonDomainService;

        public Mediator(
            OrderDomainService orderDomainService,
            CustomerDomainService customerDomainService,
            ProductDomainService productDomainService,
            InvoiceDomainService invoiceDomainService,
            DeliveryPersonDomainService deliveryPersonDomainService)
        {
            _orderDomainService = orderDomainService;
            _customerDomainService = customerDomainService;
            _productDomainService = productDomainService;
            _invoiceDomainService = invoiceDomainService;
            _deliveryPersonDomainService = deliveryPersonDomainService;
        }

        public async Task AddEntity(TEntity entity)
        {
            await (typeof(TEntity) switch
            {
                var t when t == typeof(OrderEntity) =>
                    _orderDomainService.AddOrder((entity as OrderEntity)!),
                var t when t == typeof(InvoiceEntity) =>
                    _invoiceDomainService.AddInvoice((entity as InvoiceEntity)!),
                _ => throw new ArgumentException(ErrorMessages.NotSupportedEntityType, typeof(TEntity).Name)
            });
        }
        
        public async Task<TEntity> GetEntityById(Guid id)
        {
            return typeof(TEntity) switch
            {
                var t when t == typeof(CustomerEntity) =>
                    (await _customerDomainService.GetCustomerById(id) as TEntity)!,
                var t when t == typeof(OrderEntity) =>
                    (await _orderDomainService.GetOrderById(id) as TEntity)!,
                var t when t == typeof(InvoiceEntity) =>
                    (await _invoiceDomainService.GetInvoiceById(id) as TEntity)!,
                _ =>
                    throw new ArgumentException(ErrorMessages.NotSupportedEntityType, typeof(TEntity).Name)
            };
        }

        public async Task RemoveEntityById(Guid id)
        {
            await (typeof(TEntity) switch
            {
                var t when t == typeof(OrderEntity) =>
                    _orderDomainService.RemoveOrder(id),
                var t when t == typeof(InvoiceEntity) =>
                    _invoiceDomainService.RemoveInvoice(id),
                _ => throw new ArgumentException(ErrorMessages.NotSupportedEntityType, typeof(TEntity).Name)
            });
        }

        public async Task AddDeliverySlot(Guid deliveryPersonId, DateTime deliveryTime)
        {
            await _deliveryPersonDomainService.AddDeliverySlot(deliveryPersonId, deliveryTime);
        }

        public async Task<List<ProductEntity>> GetProductsByIds(List<Guid> ids)
        {
            return await _productDomainService.GetProductsByIds(ids);
        }

        public async Task<PersonEntity> GetDeliveryPersonByTime(DateTime deliveryTime)
        {
            return await _deliveryPersonDomainService.GetDeliveryPersonByTime(deliveryTime);
        }

        public async Task<List<OrderEntity>> GetOrdersByRegionId(int regionId)
        {
            return await _orderDomainService.GetOrdersByRegionId(regionId);
        }

        public async Task UpdateOrderProducts(OrderEntity order)
        {
            await _orderDomainService.UpdateOrderProducts(order);
        }

        public void UpdateOrderStatus(OrderEntity order)
        {
            _orderDomainService.UpdateOrderStatus(order);
        }
    }
}
