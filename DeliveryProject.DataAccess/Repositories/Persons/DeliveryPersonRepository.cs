using System.Runtime.Intrinsics.Arm;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Persons
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public DeliveryPersonRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<DeliveryPersonEntity?> GetDeliveryPersonByTime(DateTime deliveryTime)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Persons
                .OfType<DeliveryPersonEntity>()
                .Include(dp => dp.AttributeValues)
                .ThenInclude(av => av.Attribute)
                .AsNoTracking()
                .Where(p => p.RoleId == (int)RoleType.DeliveryPerson)
                .Join(dbContext.DeliverySlots,
                      person => person.Id,
                      slot => slot.DeliveryPersonId,
                      (person, slot) => new { person, slot })
                .Where(ps => ps.slot.SlotTime != deliveryTime)
                .Select(ps => ps.person)
                .FirstOrDefaultAsync();
        }

        public async Task AddSlot(DeliverySlotEntity slot)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            await dbContext.DeliverySlots.AddAsync(slot);
            await dbContext.SaveChangesAsync();
        }
    }
}
