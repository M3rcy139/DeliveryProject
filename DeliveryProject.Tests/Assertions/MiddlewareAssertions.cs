using DeliveryProject.Middleware;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryProject.Tests.Assertions
{
    public static class MiddlewareAssertions
    {
        public static void AssertLogContains(Mock<ILogger<ExceptionHandlingMiddleware>> loggerMock, LogLevel expectedLogLevel, string expectedMessage, int statusCode)
        {
            loggerMock.Verify(
                log => log.Log(
                    expectedLogLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(expectedMessage) && v.ToString().Contains($"StatusCode: {statusCode}")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once, $"It was expected that a {expectedLogLevel} level message with a status code would be recorded in the log. {statusCode}");
        }
    }
}