using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Repositories.BatchUpload;
using DeliveryProject.DataAccess.Repositories.Common;
using DeliveryProject.DataAccess.Repositories.Persons;

namespace DeliveryProject.ServiceCollection
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<ISupplierRepository, SupplierRepository>();
            services.AddSingleton<IDeliveryPersonRepository, DeliveryPersonRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<IAttributeRepository, AttributeRepository>();

            services.AddSingleton<IBatchUploadRepository, BatchUploadRepository>();
        }
    }
}
