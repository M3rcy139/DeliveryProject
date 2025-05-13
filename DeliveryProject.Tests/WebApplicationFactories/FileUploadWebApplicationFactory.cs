using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DeliveryProject.Tests.WebApplicationFactories;

public class FileUploadWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IFileUploadProcessor> FileUploadProcessorMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(FileUploadProcessorMock.Object);
        });
    }
}