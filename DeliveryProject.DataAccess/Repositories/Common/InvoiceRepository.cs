using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Common;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly DeliveryDbContext _dbContext;
    
    public InvoiceRepository(DeliveryDbContext dbContext) => _dbContext = dbContext;
    
    public async Task AddInvoice(InvoiceEntity invoiceEntity)
    {
        await _dbContext.Invoices.AddAsync(invoiceEntity);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<InvoiceEntity?> GetInvoiceByOrderId(Guid orderId)
    {
        var invoice = await _dbContext.Invoices.FirstOrDefaultAsync(i => i.OrderId == orderId);

        return invoice;
    }

    public async Task UpdateInvoice(InvoiceEntity invoiceEntity)
    {
        _dbContext.Invoices.Update(invoiceEntity);

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteInvoice(Guid invoiceId)
    {
        var invoice = await _dbContext.Invoices.FindAsync(invoiceId);
        
        _dbContext.Invoices.Remove(invoice!);
        await _dbContext.SaveChangesAsync();
    }
}