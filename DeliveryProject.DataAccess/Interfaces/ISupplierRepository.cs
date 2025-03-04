using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface ISupplierRepository
    {
        Task<SupplierEntity?> GetByIdAsync(Guid id);
        Task<List<SupplierEntity?>> GetSuppliersByProductIdsAsync(List<Guid> productsIds);
    }
}
