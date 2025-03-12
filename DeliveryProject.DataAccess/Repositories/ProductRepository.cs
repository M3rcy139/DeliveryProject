using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public ProductRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<List<ProductEntity?>> GetProductsByIdAsync(List<Guid> productIds)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();

            var productsDict = await dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var result = productIds
                .Select(id => productsDict.ContainsKey(id) ? productsDict[id] : null)
                .ToList();

            return result;
        }
    }
}
