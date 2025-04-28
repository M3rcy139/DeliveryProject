using AutoMapper;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Business.Mediators;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.Business.Services;

public class DeliveryService : IDeliveryService
{
    private readonly MediatorHelper<InvoiceEntity> _invoiceMediator;
    private readonly IDeliveryTimeCalculatorService _deliveryTimeCalculatorService;
    private readonly ILogger<DeliveryService> _logger;
    private readonly IMapper _mapper;
    
    public DeliveryService(MediatorHelper<InvoiceEntity> invoiceMediator,
        IDeliveryTimeCalculatorService deliveryTimeCalculatorService, ILogger<DeliveryService> logger, IMapper mapper)
    {
        _invoiceMediator = invoiceMediator;
        _deliveryTimeCalculatorService = deliveryTimeCalculatorService;
        _logger = logger;
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
        
        await _invoiceMediator.AddEntity(invoice);
        
        _logger.LogInformation(InfoMessages.AddedInvoice, orderId);
    }

    public async Task<Invoice> GetInvoice(Guid orderId)
    {
        var invoice = await _invoiceMediator.GetEntityById(orderId);

        return _mapper.Map<Invoice>(invoice);
    }

    public async Task DeleteInvoice(Guid orderId)
    {
        await _invoiceMediator.DeleteEntityById(orderId);
        
        _logger.LogInformation(InfoMessages.DeletedInvoice, orderId);
    }
}