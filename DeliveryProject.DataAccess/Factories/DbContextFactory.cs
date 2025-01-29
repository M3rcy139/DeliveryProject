using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DeliveryProject.DataAccess.Factories
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DeliveryDbContext>, IDbContextFactory<DeliveryDbContext>
    {
        private readonly IConfiguration _configuration;
        public DbContextFactory()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public DeliveryDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public DeliveryDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();
            var connectionString = _configuration.GetConnectionString(nameof(DeliveryDbContext));

            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("DeliveryProject.Migrations"));

            return new DeliveryDbContext(optionsBuilder.Options);
        }
    }
}
