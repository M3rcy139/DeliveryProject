using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Common;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;
    
    public InvoiceRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;
    
    public async Task AddInvoice(InvoiceEntity invoiceEntity)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        await dbContext.Invoices.AddAsync(invoiceEntity);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<InvoiceEntity?> GetInvoiceByOrderId(Guid orderId)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        var invoice = await dbContext.Invoices.FirstOrDefaultAsync(i => i.OrderId == orderId);

        return invoice;
    }

    public async Task UpdateInvoice(InvoiceEntity invoiceEntity)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        dbContext.Invoices.Update(invoiceEntity);

        await dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteInvoice(Guid invoiceId)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        var invoice = await dbContext.Invoices.FindAsync(invoiceId);
        
        dbContext.Invoices.Remove(invoice!);
        await dbContext.SaveChangesAsync();
    }
}