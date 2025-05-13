using DeliveryProject.Business.DomainServices;
using DeliveryProject.Business.Mediators;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Moq;

namespace DeliveryProject.Tests.Mocks;

public class MediatorMockFactory
{
    public Mock<IOrderRepository> OrderRepositoryMock { get; } = new();
    public Mock<ICustomerRepository> CustomerRepositoryMock { get; } = new();
    public Mock<IProductRepository> ProductRepositoryMock { get; } = new();
    public Mock<IInvoiceRepository> InvoiceRepositoryMock { get; } = new();
    public Mock<IDeliveryPersonRepository> DeliveryPersonRepositoryMock { get; } = new();

    public Mediator<T> CreateMediator<T>() where T : class
    {
        var orderService = new OrderDomainService(OrderRepositoryMock.Object);
        var customerService = new CustomerDomainService(CustomerRepositoryMock.Object);
        var productService = new ProductDomainService(ProductRepositoryMock.Object);
        var invoiceService = new InvoiceDomainService(InvoiceRepositoryMock.Object);
        var deliveryPersonService = new DeliveryPersonDomainService(DeliveryPersonRepositoryMock.Object);

        return new Mediator<T>(
            orderService,
            customerService,
            productService,
            invoiceService,
            deliveryPersonService
        );
    }
}