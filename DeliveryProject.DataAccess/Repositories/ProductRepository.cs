using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public ProductRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<ProductEntity> GetProductByNameAsync(string productName)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Name == productName);
        }

        public async Task<List<ProductEntity>> GetProductsByIdAsync(List<Guid> productIds)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}
