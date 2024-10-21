using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Persistence.Interfaces;
using DeliveryProject.Persistence.Repositories;

namespace DeliveryProject.Persistence
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services
                , IConfiguration configuration)
        {
            services.AddDbContext<DeliveryDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(DeliveryDbContext)));
            });

            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
