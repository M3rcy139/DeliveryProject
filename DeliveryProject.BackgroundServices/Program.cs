using DeliveryProject.BackgroundServices.BackgroundServices;
using DeliveryProject.ServiceCollection;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;
var services = builder.Services;

builder.ConfigureBackgroundServiceLogging(configuration);

try
{
    Log.Information("Starting background service...");

    services.AddDbServices(configuration);

    services.AddProcessors();
    services.AddRepositories();

    services.AddHostedService<BatchUploadService>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The background service stopped due to an exception.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}