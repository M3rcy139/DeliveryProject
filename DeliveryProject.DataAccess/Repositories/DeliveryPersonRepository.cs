using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DeliveryProject.DataAccess.Repositories
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly DeliveryDbContext _context;

        public DeliveryPersonRepository(DeliveryDbContext context)
        {
            _context = context;
        }
        public async Task<DeliveryPersonEntity?> GetAvailableDeliveryPersonAsync(DateTime deliveryTime)
        {
            return _context.DeliveryPersons
                .AsNoTracking()
                .AsEnumerable()  
                .FirstOrDefault(d => !d.DeliverySlots.Contains(deliveryTime));
        }

        public async Task UpdateAsync(DeliveryPersonEntity deliveryPerson)
        {
            _context.DeliveryPersons.Update(deliveryPerson);
            await _context.SaveChangesAsync();
        }
    }
}
