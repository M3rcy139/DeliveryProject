using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Persons
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public CustomerRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<CustomerEntity?> GetCustomerByIdAsync(Guid personId)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Persons
                .OfType<CustomerEntity>() 
                .Include(c => c.AttributeValues)
                .ThenInclude(av => av.Attribute)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == personId);
        }
    }
}
