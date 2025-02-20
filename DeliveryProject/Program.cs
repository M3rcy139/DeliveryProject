using NLog;
using DeliveryProject.ServiceCollection;
using DeliveryProject.Bussiness.Mappings;
using DeliveryProject.Bussiness.BackgroundServices;
using DeliveryProject.DataAccess.Initializers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var logger = LogManager.GetCurrentClassLogger();

try
{
    builder.Host.ConfigureLogging(configuration);

    logger.Info("Initializing the application.");


    services.AddDbServices(configuration);
    
    services.AddFluentValidationServices();

    services.AddControllersAndSwagger();

    services.AddServices();
    services.AddRepositories();

    services.AddDataBaseInitializer();

    services.AddHostedService<BatchUploadProcessor>();

    services.AddAutoMapper(typeof(DataBaseMappings));


    var app = builder.Build();
    
    app.ConfigureMiddleware(builder.Environment);

    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await dbInitializer.InitializeDatabaseAsync();
    }

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "The application is stopped due to an exception.");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

public partial class Program { }
