using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductEntity?>> GetProductsById(List<Guid> productIds);
    }
}
