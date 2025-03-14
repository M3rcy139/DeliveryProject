using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public CustomerRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<PersonEntity?> GetCustomerByIdAsync(Guid personId)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId && p.Role.RoleType == RoleType.Customer);
        }
    }
}
