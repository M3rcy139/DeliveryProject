using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Common;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;
    
    public DeliveryRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;
    
    public async Task AddInvoice(InvoiceEntity invoiceEntity)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        await dbContext.Invoices.AddAsync(invoiceEntity);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<InvoiceEntity?> GetInvoice(Guid invoiceId)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        var invoice = await dbContext.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId);

        return invoice;
    }

    public async Task UpdateInvoice(InvoiceEntity invoiceEntity)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        var existingInvoice = await dbContext.Invoices.FindAsync(invoiceEntity.Id);

        existingInvoice!.Amount = invoiceEntity.Amount;
        dbContext.Entry(existingInvoice).Property(i => i.Amount).IsModified = true;

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