using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces;

public interface IInvoiceRepository
{
    Task AddInvoice(InvoiceEntity invoiceEntity);
    Task<InvoiceEntity?> GetInvoiceByOrderId(Guid orderId);
    Task UpdateInvoice(InvoiceEntity invoiceEntity);
    Task DeleteInvoice(Guid invoiceId);
}