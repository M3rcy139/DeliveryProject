using DeliveryProject.DataAccess.Initializers;

namespace DeliveryProject.ServiceCollection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataBaseInitializer(this IServiceCollection services)
        {
            services.AddSingleton<DatabaseInitializer>();

            return services;
        }
    }
}
