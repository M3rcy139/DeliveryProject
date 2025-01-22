using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IDeliveryPersonRepository
    {
        Task<DeliveryPersonEntity?> GetAvailableDeliveryPersonAsync(DateTime deliveryTime);
        Task UpdateAsync(DeliveryPersonEntity deliveryPerson);
    }
}
