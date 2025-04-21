using DeliveryProject.Settings;
using Serilog.Sinks.Elasticsearch;
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
            var elasticUri = configuration["ElasticConfiguration:Uri"];
            var logSettings = configuration.GetSection("Logging").Get<LoggingSettings>();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logSettings!.DefaultLogLevel)
                .MinimumLevel.Override("Microsoft.AspNetCore", logSettings.AspNetCoreLogLevel)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "DeliveryProject")
                .WriteTo.Console(restrictedToMinimumLevel: logSettings.ConsoleLogLevel)
                .WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    path: logSettings.FilePath,
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: logSettings.FileLogLevel,
                    retainedFileCountLimit: 7)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri!))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"deliveryproject-logs-{DateTime.UtcNow:yyyy-MM}",
                    MinimumLogEventLevel = logSettings.ElasticSearchLogLevel,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
                .CreateLogger();

            hostBuilder.UseSerilog();
        }
    }
}
