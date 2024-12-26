using NLog;
using NLog.Web;
using DeliveryProject.ServiceCollection;
using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Bussiness.Mappings;
using DeliveryProject.Bussiness.Services;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var logger = LogManager.GetCurrentClassLogger();

try
{
    builder.Host.ConfigureLogging(configuration);

    logger.Info("Инициализация приложения");

    services.AddDbServices(configuration);
    
    services.AddFluentValidationServices();

    services.AddRouting();
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDependencyInjection();

    services.AddAutoMapper(typeof(DataBaseMappings));

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
