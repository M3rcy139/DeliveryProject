namespace DeliveryProject.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    ICustomerRepository Customers { get; }
    IProductRepository Products { get; }

    Task<int> SaveChangesAsync();
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
