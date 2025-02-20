using DeliveryProject.Bussiness.Mediators;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Bussiness.Services;

namespace DeliveryProject.ServiceCollection
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<RepositoryMediator>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IBatchUploadService, BatchUploadService>();
        }
    }
}
