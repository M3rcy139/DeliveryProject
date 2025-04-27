namespace DeliveryProject.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    ICustomerRepository Customers { get; }
    IProductRepository Products { get; }
    IInvoiceRepository Invoices { get; }
    IDeliveryPersonRepository DeliveryPersons { get; }
    
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
