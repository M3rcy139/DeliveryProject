using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using RollingInterval = Serilog.RollingInterval;

namespace DeliveryProject.ServiceCollection
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            var mongoConnection = configuration.GetConnectionString("MongoDbLogs");
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "DeliveryProject")
                .WriteTo.Console()
                .WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    $"logs/log-{DateTime.UtcNow:yyyy-MM-dd}.json",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
                .WriteTo.MongoDBBson(
                    mongoConnection!,
                    collectionName: "applogs")
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .CreateLogger();

            hostBuilder.UseSerilog();
        }
    }
}
