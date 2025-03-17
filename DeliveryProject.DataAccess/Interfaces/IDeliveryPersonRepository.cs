using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IDeliveryPersonRepository
    {
        Task<DeliveryPersonEntity?> GetDeliveryPersonByTimeAsync(DateTime deliveryTime);
        Task AddSlotAsync(DeliverySlotEntity slot);
    }
}
