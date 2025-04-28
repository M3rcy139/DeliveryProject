using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Persons
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly DeliveryDbContext _dbContext;

        public DeliveryPersonRepository(DeliveryDbContext dbContext) => _dbContext = dbContext;

        public async Task<DeliveryPersonEntity?> GetDeliveryPersonByTime(DateTime deliveryTime)
        {
            return await _dbContext.Persons
                .OfType<DeliveryPersonEntity>()
                .AsNoTracking()
                .Join(_dbContext.DeliverySlots,
                      person => person.Id,
                      slot => slot.DeliveryPersonId,
                      (person, slot) => new { person, slot })
                .Where(ps => ps.slot.SlotTime != deliveryTime)
                .Select(ps => ps.person)
                .FirstOrDefaultAsync();
        }

        public async Task AddSlot(DeliverySlotEntity slot)
        {
            await _dbContext.DeliverySlots.AddAsync(slot);
        }
    }
}
