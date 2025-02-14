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

            services.AddDbContextOptions<DeliveryDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("DeliveryProject.Migrations"))
            );

            services.AddSingleton<IDbContextFactory<DeliveryDbContext>>(provider =>
            {
                var options = provider.GetRequiredService<DbContextOptions<DeliveryDbContext>>();
                return new PooledDbContextFactory<DeliveryDbContext>(options);
            });
        }

        private static void AddDbContextOptions<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            services.AddSingleton(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TContext>();
                optionsAction(optionsBuilder);
                return optionsBuilder.Options;
            });
        }
    }
}
