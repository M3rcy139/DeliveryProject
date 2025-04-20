using DeliveryProject.Core.Models;

namespace DeliveryProject.Business.Interfaces.Services;

public interface IDeliveryService
{
    Task AddInvoice(Guid orderId);
    Task<Invoice> GetInvoice(Guid orderId);
    Task DeleteInvoice(Guid orderId);
}