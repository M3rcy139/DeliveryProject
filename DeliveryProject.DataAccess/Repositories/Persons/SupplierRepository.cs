using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Persons
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public SupplierRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<SupplierEntity?> GetByIdAsync(Guid id)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Persons.OfType<SupplierEntity>()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SupplierEntity>> GetSuppliersByProductIdsAsync(List<Guid> productIds)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Supplier)
                .OfType<SupplierEntity>()
                .Where(s => s.Role.RoleType == RoleType.Supplier)
                .Distinct()
                .ToListAsync();
        }
    }
}

