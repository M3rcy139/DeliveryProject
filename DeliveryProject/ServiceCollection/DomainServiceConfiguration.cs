using DeliveryProject.Business.DomainServices;

namespace DeliveryProject.ServiceCollection;

public static class DomainServiceConfiguration
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<CustomerDomainService>();
        services.AddSingleton<DeliveryPersonDomainService>();
        services.AddSingleton<InvoiceDomainService>();
        services.AddSingleton<OrderDomainService>();
        services.AddSingleton<ProductDomainService>();
    }
}