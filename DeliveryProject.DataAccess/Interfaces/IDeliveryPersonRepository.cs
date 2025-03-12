using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IDeliveryPersonRepository
    {
        Task<PersonEntity?> GetAvailableDeliveryPersonAsync(DateTime deliveryTime);
        Task AddSlotAsync(DeliverySlotEntity slot);
    }
}
