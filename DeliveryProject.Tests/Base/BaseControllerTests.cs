using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.DataAccess.Interfaces;
using Moq;

namespace DeliveryProject.Tests.Base
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient _client;
        protected readonly Mock<IOrderService> _orderServiceMock;
        protected readonly Mock<IOrderRepository> _orderRepositoryMock;

        protected BaseControllerTests(HttpClient client, Mock<IOrderService> orderServiceMock,
            Mock<IOrderRepository> orderRepositoryMock)
        {
            _client = client;
            _orderServiceMock = orderServiceMock;
            _orderRepositoryMock = orderRepositoryMock;
        }

        protected async Task<HttpResponseMessage> GetAsync(string relativeUrl)
        {
            var baseUrl = "/api/order";
            return await _client.GetAsync($"{baseUrl}/{relativeUrl.TrimStart('/')}");
        }
    }
}
