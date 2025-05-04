using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Persons
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DeliveryDbContext _dbContext;

        public CustomerRepository(DeliveryDbContext dbContext) => _dbContext = dbContext;

        public async Task<CustomerEntity?> GetCustomerById(Guid personId)
        {
            return await _dbContext.Persons
                .OfType<CustomerEntity>() 
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == personId);
        }
    }
}
