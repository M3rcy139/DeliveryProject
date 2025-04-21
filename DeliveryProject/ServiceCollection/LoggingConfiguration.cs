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
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri!))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"deliveryproject-logs-{DateTime.UtcNow:yyyy-MM}",
                    MinimumLogEventLevel = LogEventLevel.Information,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
                .CreateLogger();

            hostBuilder.UseSerilog();
        }
    }
}
