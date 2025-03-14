using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface ISupplierRepository
    {
        Task<PersonEntity?> GetByIdAsync(Guid id);
        Task<List<PersonEntity>> GetSuppliersByProductIdsAsync(List<Guid> productIds);
    }
}
