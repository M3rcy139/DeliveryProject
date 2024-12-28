using NLog.Web;
using NLog;

namespace DeliveryProject.ServiceCollection
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            LogManager.Setup().LoadConfigurationFromAppSettings();

            hostBuilder.ConfigureLogging(logging =>
            {
                logging.ClearProviders(); 
                logging.AddConfiguration(configuration.GetSection("Logging"));
                logging.AddConsole();
            });

            hostBuilder.UseNLog();
        }
    }
}
