using AutoMapper;
using DeliveryProject.Bussiness.Helpers;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Bussiness.Mediators;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Services;

public class DeliveryService : IDeliveryService
{
    private readonly MediatorHelper<InvoiceEntity> _invoiceMediator;
    private readonly IDeliveryTimeCalculatorService _deliveryTimeCalculatorService;
    private readonly IMapper _mapper;
    
    public DeliveryService(MediatorHelper<InvoiceEntity> invoiceMediator, IOrderService orderService,
        IDeliveryTimeCalculatorService deliveryTimeCalculatorService, IMapper mapper)
    {
        _invoiceMediator = invoiceMediator;
        _deliveryTimeCalculatorService = deliveryTimeCalculatorService;
        _mapper = mapper;
    }
    
    public async Task AddInvoice(Guid orderId)
    {
        var deliveryTime = _deliveryTimeCalculatorService.CalculateDeliveryTime();

        var invoice =  new InvoiceEntity
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            DeliveryTime = deliveryTime.ToUniversalTime(),
            IsExecuted = false
        };
        
        await _invoiceMediator.AddInvoice(invoice);
    }

    public async Task<Invoice> GetInvoice(Guid orderId)
    {
        var invoice = await _invoiceMediator.GetEntityById(orderId);

        return _mapper.Map<Invoice>(invoice);
    }

    public async Task DeleteInvoice(Guid orderId)
    {
        await _invoiceMediator.DeleteEntityById(orderId);
    }
}