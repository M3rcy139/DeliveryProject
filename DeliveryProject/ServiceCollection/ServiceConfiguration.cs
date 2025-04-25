using DeliveryProject.Business.Mediators;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Business.Services;

namespace DeliveryProject.ServiceCollection
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IDeliveryTimeCalculatorService, DeliveryTimeCalculatorService>();
            services.AddScoped(typeof(MediatorHelper<>));
        }
    }
}
