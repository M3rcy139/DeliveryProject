using AutoMapper;
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
    private readonly IMapper _mapper;
    
    public DeliveryService(RepositoryMediator repositoryMediator, IDeliveryTimeCalculatorService deliveryTimeCalculatorService
    , IMapper mapper)
    {
        _repositoryMediator = repositoryMediator;
        _deliveryTimeCalculatorService = deliveryTimeCalculatorService;
        _mapper = mapper;
    }
    
    public async Task AddInvoice(Order order)
    {
        var deliveryTime = _deliveryTimeCalculatorService.CalculateDeliveryTime();

        var invoice =  new InvoiceEntity
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            DeliveryTime = deliveryTime.ToUniversalTime(),
            IsExecuted = false
        };

        await _repositoryMediator.AddInvoice(invoice);
    }

    public async Task<Invoice> GetInvoice(Guid orderId)
    {
        var invoice = await _repositoryMediator.GetInvoiceByOrderId(orderId);

        return _mapper.Map<Invoice>(invoice);
    }

    public async Task DeleteInvoice(Guid orderId)
    {
        await _repositoryMediator.DeleteInvoice(orderId);
    }
}