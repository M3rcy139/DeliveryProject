using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.ServiceCollection
{
    public static class DbConfiguration
    {
        public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DeliveryDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString(nameof(DeliveryDbContext)),
                    b => b.MigrationsAssembly("DeliveryProject.Migrations"));
            }, ServiceLifetime.Scoped);
        }
    }
}
