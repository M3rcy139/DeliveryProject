using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Repositories;

namespace DeliveryProject.ServiceCollection
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<ISupplierRepository, SupplierRepository>();
            services.AddSingleton<IDeliveryPersonRepository, DeliveryPersonRepository>();
            services.AddSingleton<IBatchUploadRepository, BatchUploadRepository>();
        }
    }
}
