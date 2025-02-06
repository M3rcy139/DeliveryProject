using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Factories
{
    public class DbContextFactory : IDbContextFactory<DeliveryDbContext>
    {
        private readonly DbContextOptions<DeliveryDbContext> _options;

        public DbContextFactory(DbContextOptions<DeliveryDbContext> options)
        {
            _options = options;
        }

        public DeliveryDbContext CreateDbContext()
        {
            return new DeliveryDbContext(_options);
        }
    }
}
