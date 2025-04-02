using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Interfaces.Services;

public interface IDeliveryService
{
    Task AddInvoice(Order order);
    Task<Invoice> GetInvoice(Guid invoiceId);
    Task UpdateInvoice(Invoice invoice);
    Task DeleteInvoice(Guid invoiceId);
}