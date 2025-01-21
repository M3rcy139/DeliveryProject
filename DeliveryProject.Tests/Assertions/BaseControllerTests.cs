using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Dto;
using DeliveryProject.DataAccess.Interfaces;
using FluentAssertions;
using Moq;

namespace DeliveryProject.Tests.Assertions
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

        public void AssertResponseDetails(CustomProblemDetails responseContent, string expectedMessage, string expectedMethodName)
        {
            responseContent.Detail.Should().NotBeNullOrEmpty("Ошибка должна содержать подробности.");
            responseContent.Detail.Should().Contain(expectedMessage, $"Ошибка должна содержать сообщение: {expectedMessage}");
            responseContent.Detail.Should().Contain(expectedMethodName, $"Ошибка должна содержать метод: {expectedMethodName}");
        }
    }
}
