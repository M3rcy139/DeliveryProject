using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Dto;
using DeliveryProject.Tests.Mocks;
using FluentAssertions;
using Moq;

namespace DeliveryProject.Tests.Assertions
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient _client;
        protected readonly Mock<IOrderService> _orderServiceMock;

        protected BaseControllerTests(HttpClient client, Mock<IOrderService> orderServiceMock)
        {
            _client = client;
            _orderServiceMock = orderServiceMock;
        }
        protected async Task<HttpResponseMessage> GetAsync(string relativeUrl)
        {
            var baseUrl = "/api/order";
            return await _client.GetAsync($"{baseUrl}/{relativeUrl.TrimStart('/')}");
        }

        public void AssertResponseDetails(CustomProblemDetails responseContent, string expectedMessage, string expectedMethodName)
        {
            responseContent.Detail.Should().NotBeNullOrEmpty("Ошибка должна содержать подробности.");
            responseContent.Detail.Should().Contain(expectedMessage, $"Ошибка должна содержать сообщение: {expectedMessage}");
            responseContent.Detail.Should().Contain(expectedMethodName, $"Ошибка должна содержать метод: {expectedMethodName}");
        }
    }
}
