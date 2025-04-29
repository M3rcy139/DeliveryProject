using DeliveryProject.Business.Mediators;
using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.Business.Services;

namespace DeliveryProject.ServiceCollection
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<IDeliveryService, DeliveryService>();
            services.AddSingleton<IDeliveryTimeCalculatorService, DeliveryTimeCalculatorService>();
            services.AddSingleton(typeof(MediatorHelper<>));
        }
    }
}
