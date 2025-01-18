using Moq;
using Microsoft.Extensions.Logging;
using DeliveryProject.Middleware;

namespace DeliveryProject.Tests.Assertions
{
    public abstract class BaseMiddlewareTestAssertions
    {
        public void AssertLogContains(Mock<ILogger<ExceptionHandlingMiddleware>> loggerMock, LogLevel expectedLogLevel, string expectedMessage, int statusCode)
        {
            loggerMock.Verify(
                log => log.Log(
                    expectedLogLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(expectedMessage) && v.ToString().Contains($"StatusCode: {statusCode}")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once, $"Ожидалось, что в лог будет записано сообщение уровня {expectedLogLevel} со статус-кодом {statusCode}");
        }
    }
}
