using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IPersonRepository
    {
        Task<PersonEntity?> GetCustomerByIdAndRoleAsync(Guid personId, RoleType role);
        Task<List<PersonEntity>> GetPersonsByProductIdsAndRoleAsync(List<Guid> productIds, RoleType role);
        Task<PersonEntity?> GetAvailableDeliveryPersonAsync(DateTime deliveryTime);
    }
}
