using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public SupplierRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<PersonEntity?> GetByIdAsync(Guid id)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Persons.FirstOrDefaultAsync(s => s.Id == id);
        }

        //public async Task<List<SupplierEntity?>> GetSuppliersByProductIdsAsync(List<Guid> productIds)
        //{
        //    await using var dbContext = await _contextFactory.CreateDbContextAsync();

        //    var suppliers = await dbContext.Suppliers
        //        .Where(s => s.Products.Any(p => productIds.Contains(p.Id)))
        //        .Select(s => new
        //        {
        //            Supplier = s,
        //            ProductIds = s.Products.Select(p => p.Id).ToList() 
        //        })
        //        .ToListAsync();

        //    var suppliersDict = suppliers
        //        .SelectMany(s => s.ProductIds.Select(pid => new { ProductId = pid, s.Supplier }))
        //        .GroupBy(x => x.ProductId)
        //        .ToDictionary(g => g.Key, g => g.First().Supplier);

        //    var result = productIds
        //        .Select(id => suppliersDict.TryGetValue(id, out var supplier) ? supplier : null)
        //        .ToList();

        //    return result;
        //}

        public async Task<List<PersonEntity>> GetPersonsByProductIdsAndRoleAsync(List<Guid> productIds)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Supplier)
                .Where(s => s.Role.RoleType == RoleType.Supplier)
                .Distinct()
                .ToListAsync();
        }
    }
}

