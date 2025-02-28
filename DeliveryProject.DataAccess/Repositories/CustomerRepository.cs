using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public CustomerRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<CustomerEntity?> GetCustomerByIdAsync(Guid customerId)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }
    }
}
