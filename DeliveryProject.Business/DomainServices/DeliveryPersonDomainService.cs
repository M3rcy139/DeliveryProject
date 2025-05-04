using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Business.DomainServices;

public class DeliveryPersonDomainService
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;

    public DeliveryPersonDomainService(IDeliveryPersonRepository deliveryPersonRepository) 
        => _deliveryPersonRepository = deliveryPersonRepository;
    
    public async Task AddDeliverySlot(Guid deliveryPersonId, DateTime deliveryTime)
    {
        await _deliveryPersonRepository.AddSlot(new DeliverySlotEntity
        {
            DeliveryPersonId = deliveryPersonId,
            SlotTime = deliveryTime
        });
    }
    
    public async Task<PersonEntity> GetDeliveryPersonByTime(DateTime deliveryTime)
    {
        var availableDeliveryPerson = await _deliveryPersonRepository
            .GetDeliveryPersonByTime(deliveryTime);

        availableDeliveryPerson.ValidateEntity(ErrorMessages.NoAvailableDeliveryPersons,
            ErrorCodes.NoAvailableDeliveryPersons);

        return availableDeliveryPerson!;
    }
}