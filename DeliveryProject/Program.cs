using DeliveryProject.ServiceCollection;
using DeliveryProject.Business.Mappings;
using DeliveryProject.BackgroundServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Host.ConfigureLogging(configuration);

try
{
    Log.Information("Initializing the application.");


    services.AddDbServices(configuration);
    
    services.AddFluentValidationServices();

    services.AddControllersAndSwagger();

    services.AddServices();
    services.AddDomainServices();
    services.AddProcessors();
    services.AddRepositories();

    services.AddHostedService<BatchUploadService>();

    services.AddAutoMapper(typeof(DataBaseMappings));


    var app = builder.Build();
    
    app.ConfigureMiddleware(builder.Environment);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application is stopped due to an exception.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
