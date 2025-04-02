using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces;

public interface IDeliveryRepository
{
    Task AddInvoice(InvoiceEntity invoiceEntity);
    Task<InvoiceEntity?> GetInvoice(Guid invoiceId);
    Task DeleteInvoice(Guid invoiceId);
}