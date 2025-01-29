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
            using var dbContext = _contextFactory.CreateDbContext();
            var deliveryPersons = await dbContext.DeliveryPersons
                .AsNoTracking()
                .ToListAsync(); 

            return deliveryPersons
                .FirstOrDefault(d => !d.DeliverySlots.Contains(deliveryTime));
        }

        public async Task UpdateAsync(DeliveryPersonEntity deliveryPerson)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            dbContext.DeliveryPersons.Update(deliveryPerson);
            await dbContext.SaveChangesAsync();
        }
    }
}
