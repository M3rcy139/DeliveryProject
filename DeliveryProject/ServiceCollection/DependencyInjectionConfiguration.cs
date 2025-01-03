using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Bussiness.Services;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Repositories;

namespace DeliveryProject.ServiceCollection
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
        }
    }
}
