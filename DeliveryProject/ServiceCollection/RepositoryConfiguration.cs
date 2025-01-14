using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Repositories;

namespace DeliveryProject.ServiceCollection
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
        }
    }
}
