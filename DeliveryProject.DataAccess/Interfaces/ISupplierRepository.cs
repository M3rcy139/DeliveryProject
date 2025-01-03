using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface ISupplierRepository
    {
        Task<SupplierEntity?> GetByIdAsync(int id);
    }
}
