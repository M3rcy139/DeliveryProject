using Moq;
using Microsoft.Extensions.Logging;
using DeliveryProject.Middleware;

namespace DeliveryProject.Tests.Base
{
    public abstract class BaseMiddlewareTest
    {
        protected readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;

        protected BaseMiddlewareTest()
        {
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        }
    }
}
