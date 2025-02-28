using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity> GetProductByNameAsync(string productName);
        Task<List<ProductEntity>> GetProductsByIdAsync(List<Guid> productIds);
    }
}
