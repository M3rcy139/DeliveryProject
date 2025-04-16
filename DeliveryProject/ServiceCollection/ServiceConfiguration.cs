using DeliveryProject.Bussiness.Mediators;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Bussiness.Services;
using DeliveryProject.DataAccess.Factories;

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
