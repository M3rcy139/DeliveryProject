using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DeliveryProject.ServiceCollection
{
    public static class DbConfiguration
    {
        public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(DeliveryDbContext));
            
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();
            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("DeliveryProject.Migrations"));

            var dbContextOptions = optionsBuilder.Options;
            
            services.AddSingleton(dbContextOptions);
            services.AddSingleton<DeliveryDbContext>(provider =>
            {
                var options = provider.GetRequiredService<DbContextOptions<DeliveryDbContext>>();
                return new DeliveryDbContext(options);
            });
            
            services.AddSingleton<IDbContextFactory<DeliveryDbContext>>(
                provider => new PooledDbContextFactory<DeliveryDbContext>(
                    new DbContextOptionsBuilder<DeliveryDbContext>()
                        .UseNpgsql(configuration.GetConnectionString(nameof(DeliveryDbContext)))
                        .Options
                )
            );
        }
    }
}
