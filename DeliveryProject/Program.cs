using NLog;
using DeliveryProject.ServiceCollection;
using DeliveryProject.Bussiness.Mappings;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var logger = LogManager.GetCurrentClassLogger();

try
{
    builder.Host.ConfigureLogging(configuration);

    logger.Info("������������� ����������");

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
    logger.Error(ex, "���������� ����������� ��-�� ����������.");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

public partial class Program { }
