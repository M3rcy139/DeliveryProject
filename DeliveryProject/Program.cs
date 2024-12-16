using NLog;
using NLog.Web;
using DeliveryProject.API.Middleware;
using DeliveryProject.ServiceCollection;
using DeliveryProject;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

LogManager.Setup().LoadConfigurationFromAppSettings();
var logger = LogManager.GetCurrentClassLogger();
logger.Info("Инициализация приложения");

try
{
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    services.ConfigureServices(configuration);

    var app = builder.Build();

    app.ConfigureMiddleware(builder.Environment);

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Приложение остановлено из-за исключения.");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

public partial class Program { }
