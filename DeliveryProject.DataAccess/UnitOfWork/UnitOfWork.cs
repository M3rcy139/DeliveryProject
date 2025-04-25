using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeliveryProject.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DeliveryDbContext _context;
    private IDbContextTransaction _transaction;

    public IOrderRepository Orders { get; }
    public ICustomerRepository Customers { get; }
    public IProductRepository Products { get; }

    public UnitOfWork(
        DeliveryDbContext context,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        _context = context;
        Orders = orderRepository;
        Customers = customerRepository;
        Products = productRepository;
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction?.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await DisposeTransactionAsync();
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
        _transaction?.Dispose();
    }
}
