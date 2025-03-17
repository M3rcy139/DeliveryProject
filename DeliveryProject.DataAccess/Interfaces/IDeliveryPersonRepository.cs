using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IDeliveryPersonRepository
    {
        Task<DeliveryPersonEntity?> GetDeliveryPersonByTime(DateTime deliveryTime);
        Task AddSlot(DeliverySlotEntity slot);
    }
}
