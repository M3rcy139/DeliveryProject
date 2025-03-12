using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface ISupplierRepository
    {
        Task<PersonEntity?> GetByIdAsync(Guid id);
        //Task<List<SupplierEntity?>> GetSuppliersByProductIdsAsync(List<Guid> productsIds);
        Task<List<PersonEntity>> GetPersonsByProductIdsAndRoleAsync(List<Guid> productIds);
    }
}
