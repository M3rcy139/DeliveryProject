using AutoMapper;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Extensions;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.Business.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryTimeCalculatorService _deliveryTimeCalculatorService;
    private readonly ILogger<DeliveryService> _logger;
    private readonly IMapper _mapper;
    
    public DeliveryService(IUnitOfWork unitOfWork,
        IDeliveryTimeCalculatorService deliveryTimeCalculatorService, ILogger<DeliveryService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _deliveryTimeCalculatorService = deliveryTimeCalculatorService;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task AddInvoice(Guid orderId)
    {
        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            var deliveryTime = _deliveryTimeCalculatorService.CalculateDeliveryTime();

            var invoice = new InvoiceEntity
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                DeliveryTime = deliveryTime.ToUniversalTime(),
                IsExecuted = false
            };

            await _unitOfWork.Invoices.AddInvoice(invoice);

            _logger.LogInformation(InfoMessages.AddedInvoice, orderId);
        }, _logger);
    }

    public async Task<Invoice> GetInvoice(Guid orderId)
    {
        return await _unitOfWork.ExecuteInTransaction(async () =>
        {
            var invoice = await _unitOfWork.Invoices.GetInvoiceByOrderId(orderId);

            return _mapper.Map<Invoice>(invoice);
        }, _logger);
    }

    public async Task DeleteInvoice(Guid orderId)
    {
        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _unitOfWork.Invoices.DeleteInvoice(orderId);

            _logger.LogInformation(InfoMessages.DeletedInvoice, orderId);
        }, _logger);
    }
}