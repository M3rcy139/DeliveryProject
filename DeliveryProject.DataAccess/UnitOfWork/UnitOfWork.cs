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
    public IInvoiceRepository Invoices { get; }
    public IDeliveryPersonRepository DeliveryPersons { get; }

    public UnitOfWork(
        DeliveryDbContext context,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IInvoiceRepository invoiceRepository,
        IDeliveryPersonRepository deliveryPersonRepository)
    {
        _context = context;
        Orders = orderRepository;
        Customers = customerRepository;
        Products = productRepository;
        Invoices = invoiceRepository;
        DeliveryPersons = deliveryPersonRepository;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.SaveChangesAsync();
        await _transaction?.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
        _transaction?.Dispose();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
