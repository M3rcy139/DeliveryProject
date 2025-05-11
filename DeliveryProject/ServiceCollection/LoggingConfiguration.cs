using DeliveryProject.Settings;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Serilog.Formatting.Json;
using RollingInterval = Serilog.RollingInterval;

namespace DeliveryProject.ServiceCollection
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            ConfigureLogger(configuration);
            hostBuilder.UseSerilog();
        }

        public static void ConfigureBackgroundServiceLogging(this HostApplicationBuilder builder, IConfiguration configuration)
        {
            ConfigureLogger(configuration);
            
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();
        }
        
        private static void ConfigureLogger(IConfiguration configuration)
        {
            var logSettings = configuration.GetSection("Logging").Get<LoggingSettings>();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logSettings!.DefaultLogLevel)
                .MinimumLevel.Override("Microsoft.AspNetCore", logSettings.AspNetCoreLogLevel)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", logSettings.ApplicationName)
                .WriteTo.Console(restrictedToMinimumLevel: logSettings.ConsoleLogLevel)
                .WriteTo.File(
                    new JsonFormatter(),
                    path: logSettings.FilePath,
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: logSettings.FileLogLevel,
                    retainedFileCountLimit: logSettings.RetainedFileCountLimit)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = logSettings.ElasticIndexFormat,
                    MinimumLogEventLevel = logSettings.ElasticSearchLogLevel,
                    NumberOfShards = logSettings.ElasticSearchNumberOfShards,
                    NumberOfReplicas = logSettings.ElasticSearchNumberOfReplicas,
                })
                .CreateLogger();
        }
    }
}
