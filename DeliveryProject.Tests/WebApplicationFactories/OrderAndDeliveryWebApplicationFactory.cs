using DeliveryProject.Business.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DeliveryProject.Tests.WebApplicationFactories;

public class OrderAndDeliveryWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IOrderService> OrderServiceMock { get; } = new();
    public Mock<IDeliveryService> DeliveryServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(OrderServiceMock.Object);
            services.AddSingleton(DeliveryServiceMock.Object);
        });
    }
}