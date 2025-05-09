using DeliveryProject.Core.Settings;

namespace DeliveryProject.ServiceCollection;

public static class SettingsConfiguration
{
    public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrderSettings>(configuration.GetSection("OrderSettings"));
    }
}