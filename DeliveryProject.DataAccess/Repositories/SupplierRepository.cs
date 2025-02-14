using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public SupplierRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<SupplierEntity?> GetByIdAsync(int id)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        }  
    }
}

