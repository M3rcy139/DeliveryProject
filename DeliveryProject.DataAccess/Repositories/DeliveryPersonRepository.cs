using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public DeliveryPersonRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;
        
        public async Task<DeliveryPersonEntity?> GetAvailableDeliveryPersonAsync(DateTime deliveryTime)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();

            return await dbContext.DeliveryPersons
                .Include(dp => dp.DeliverySlots)
                .AsNoTracking()
                .FirstOrDefaultAsync(dp => !dp.DeliverySlots.Any(ds => ds.SlotTime == deliveryTime));
        }

        public async Task AddSlotAsync(DeliverySlotEntity slot)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            await dbContext.DeliverySlots.AddAsync(slot);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeliveryPersonEntity deliveryPerson)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            dbContext.DeliveryPersons.Update(deliveryPerson);
            await dbContext.SaveChangesAsync();
        }
    }
}
