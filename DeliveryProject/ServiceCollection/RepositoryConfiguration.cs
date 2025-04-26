using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Interfaces.BatchUploads;
using DeliveryProject.DataAccess.Repositories.Attributes;
using DeliveryProject.DataAccess.Repositories.BatchUpload;
using DeliveryProject.DataAccess.Repositories.Common;
using DeliveryProject.DataAccess.Repositories.Orders;
using DeliveryProject.DataAccess.Repositories.Persons;
using DeliveryProject.DataAccess.UnitOfWork;

namespace DeliveryProject.ServiceCollection
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAttributeRepository, AttributeRepository>();
            services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();

            services.AddScoped<IBatchUploadRepository, BatchUploadRepository>();
        }
    }
}
