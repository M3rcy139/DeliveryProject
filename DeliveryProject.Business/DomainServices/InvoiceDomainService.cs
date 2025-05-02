using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Business.DomainServices;

public class InvoiceDomainService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceDomainService(IInvoiceRepository invoiceRepository)
        => _invoiceRepository = invoiceRepository;
    
    public async Task AddInvoice(InvoiceEntity invoiceEntity)
    {
        await _invoiceRepository.AddInvoice(invoiceEntity);
    }
    
    public async Task<InvoiceEntity> GetInvoiceById(Guid orderId)
    {
        var invoice = await _invoiceRepository.GetInvoiceByOrderId(orderId);
        invoice.ValidateEntity(ErrorMessages.InvoiceNotFound, ErrorCodes.InvoiceNotFound);
            
        return invoice!;
    }

    public async Task RemoveInvoice(Guid orderId)
    {
        var invoice = await _invoiceRepository.GetInvoiceByOrderId(orderId);
        invoice.ValidateEntity(ErrorMessages.InvoiceNotFound, ErrorCodes.InvoiceNotFound);
        
        await _invoiceRepository.RemoveInvoice(invoice!.Id);
    }
}