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

            services.AddDbContext<DeliveryDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("DeliveryProject.Migrations"))
            );

            services.AddScoped<IDbContextFactory<DeliveryDbContext>>(provider =>
            {
                var options = provider.GetRequiredService<DbContextOptions<DeliveryDbContext>>();
                return new PooledDbContextFactory<DeliveryDbContext>(options);
            });
        }
    }
}
