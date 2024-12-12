using DeliveryProject.Access;
using DeliveryProject.Access.Mappings;
using DeliveryProject.Application;
using NLog;
using NLog.Web;
using DeliveryProject.API.Middleware;

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

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddApplication(builder.Configuration);
    services.AddPersistence(builder.Configuration);
    services.AddAutoMapper(typeof(DataBaseMappings));
  

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.MapControllers();

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
