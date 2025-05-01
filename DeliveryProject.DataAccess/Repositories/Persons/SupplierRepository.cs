using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Persons
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DeliveryDbContext _dbContext;
        
        public SupplierRepository(DeliveryDbContext dbContext) => _dbContext = dbContext;

        public async Task<SupplierEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Persons.OfType<SupplierEntity>()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SupplierEntity>> GetSuppliersByProductIdsAsync(List<Guid> productIds)
        {
            var supplierIds = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.SupplierId)
                .Distinct()
                .ToListAsync();

            return await _dbContext.Persons
                .OfType<SupplierEntity>()
                .Where(s => supplierIds.Contains(s.Id))
                .ToListAsync();
        }
    }
}

