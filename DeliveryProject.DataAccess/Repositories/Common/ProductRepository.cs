using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Common
{
    public class ProductRepository : IProductRepository
    {
        private readonly DeliveryDbContext _dbContext;

        public ProductRepository(DeliveryDbContext dbContext) => _dbContext = dbContext;

        public async Task<List<ProductEntity?>> GetProductsById(List<Guid> productIds)
        {
            var productsDict = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var result = productIds
                .Select(id => productsDict.ContainsKey(id) ? productsDict[id] : null)
                .ToList();

            return result;
        }
    }
}
