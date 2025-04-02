using DeliveryProject.Bussiness.Helpers;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Bussiness.Mediators;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Services;

public class DeliveryService : IDeliveryService
{
    private readonly RepositoryMediator _repositoryMediator;
    private readonly IDeliveryTimeCalculatorService _deliveryTimeCalculatorService;
    
    public DeliveryService(RepositoryMediator repositoryMediator, IDeliveryTimeCalculatorService deliveryTimeCalculatorService)
    {
        _repositoryMediator = repositoryMediator;
        _deliveryTimeCalculatorService = deliveryTimeCalculatorService;
    }
    
    public async Task AddInvoice(Order order)
    {
        decimal amount = OrderAmountCalculator.CalculateOrderAmount(order.OrderProducts.ToList());

        var deliveryTime = _deliveryTimeCalculatorService.CalculateDeliveryTime();

        var invoice =  new InvoiceEntity
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Amount = amount,
            DeliveryTime = deliveryTime.ToUniversalTime(),
            IsExecuted = false
        };

        await _repositoryMediator.AddInvoice(invoice);
    }

    public async Task<Invoice> GetInvoice(Guid invoiceId)
    {
        var invoice = await _repositoryMediator.GetInvoice(invoiceId);

        return invoice;
    }

    public async Task UpdateInvoice(Invoice invoice)
    {
        await _repositoryMediator.UpdateInvoice(invoice);
    }

    public async Task DeleteInvoice(Guid invoiceId)
    {
        await _repositoryMediator.DeleteInvoice(invoiceId);
    }
}