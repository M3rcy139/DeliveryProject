using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public PersonRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<PersonEntity?> GetCustomerByIdAndRoleAsync(Guid personId, RoleType role)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId && p.RoleId == (int)role);
        }

        public async Task<List<PersonEntity>> GetPersonsByProductIdsAndRoleAsync(List<Guid> productIds, RoleType role)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Supplier)
                .Where(s => s.RoleId == (int)role)
                .Distinct()
                .ToListAsync();
        }

        public async Task<PersonEntity?> GetAvailableDeliveryPersonAsync(DateTime deliveryTime)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Persons
                .Where(p => p.RoleId == (int)RoleType.DeliveryPerson)
                .Join(dbContext.DeliverySlots,
                      person => person.Id,
                      slot => slot.DeliveryPersonId,
                      (person, slot) => new { person, slot })
                .Where(ps => ps.slot.SlotTime == deliveryTime)
                .Select(ps => ps.person)
                .FirstOrDefaultAsync();
        }
    }
}
