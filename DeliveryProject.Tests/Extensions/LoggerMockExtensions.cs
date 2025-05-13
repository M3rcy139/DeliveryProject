using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryProject.Tests.Extensions;


internal static class LoggerMockExtensions
{
    public static void VerifyLogContains(this Mock<ILogger> loggerMock, string contains, LogLevel level = LogLevel.Information)
    {
        loggerMock.Verify(x => x.Log(
            level,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(contains)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ));
    }

    public static void VerifyLogContains<T>(this Mock<ILogger<T>> loggerMock, string contains, LogLevel level = LogLevel.Information)
    {
        loggerMock.As<ILogger>().VerifyLogContains(contains, level);
    }
}