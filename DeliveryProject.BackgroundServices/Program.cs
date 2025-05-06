using DeliveryProject.BackgroundServices.BackgroundServices;
using DeliveryProject.ServiceCollection;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
var configuration = builder.Configuration;
var services = builder.Services;

services.AddDbServices(configuration);

services.AddProcessors();
services.AddRepositories();

services.AddHostedService<BatchUploadService>();

var host = builder.Build();
host.Run();