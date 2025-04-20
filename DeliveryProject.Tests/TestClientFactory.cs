using DeliveryProject.Business.Interfaces.Services;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.Tests.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryProject.Tests
{
    public static class TestClientFactory
    {
        public static HttpClient CreateClient(WebApplicationFactory<Program> factory,
            out Mock<IOrderService> orderServiceMock, out Mock<IOrderRepository> orderRepositoryMock)
        {
            var localOrderServiceMock = OrderServiceMock.Create();
            var localOrderRepositoryMock = OrderRepositoryMock.Create();
            orderServiceMock = localOrderServiceMock;
            orderRepositoryMock = localOrderRepositoryMock;

            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IOrderService>(_ => localOrderServiceMock.Object);
                    services.AddSingleton<IOrderRepository>(_ => localOrderRepositoryMock.Object);
                    services.AddLogging();
                });
            }).CreateClient();
        }
    }

}
