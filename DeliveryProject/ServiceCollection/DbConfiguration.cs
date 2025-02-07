using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;

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

            services.AddSingleton<IDbContextFactory<DeliveryDbContext>, DbContextFactory>();
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
