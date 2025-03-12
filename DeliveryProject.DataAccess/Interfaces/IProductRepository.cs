using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductEntity?>> GetProductsByIdAsync(List<Guid> productIds);
    }
}
