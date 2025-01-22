using DeliveryProject.Middleware;
using DeliveryProject.Tests.Assertions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class ExceptionHandlingMiddlewareTests : BaseMiddlewareTestAssertions
{
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
    private readonly Mock<RequestDelegate> _nextMock;

    public ExceptionHandlingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _nextMock = new Mock<RequestDelegate>();
    }

    [Fact]
    public async Task InvokeAsync_FinallyExecutes_OnSuccessfulRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextMiddleware = new RequestDelegate(async (ctx) => await Task.CompletedTask);
        var middleware = new ExceptionHandlingMiddleware(nextMiddleware, _loggerMock.Object, Mock.Of<IWebHostEnvironment>());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        AssertLogContains(
            _loggerMock,
            LogLevel.Information,
            "Completion of request processing. Path: , Method: ,",
            200
        );
    }

    [Fact]
    public async Task InvokeAsync_FinallyExecutes_WhenValidationExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextMiddleware = new RequestDelegate(ctx => throw new ValidationException("Validation failed:"));
        var middleware = new ExceptionHandlingMiddleware(nextMiddleware, _loggerMock.Object, Mock.Of<IWebHostEnvironment>());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        AssertLogContains(
            _loggerMock,
            LogLevel.Information,
            $"Completion of request processing. Path: {context.Request.Path}, Method: {context.Request.Method},",
            400
        );
    }

    [Fact]
    public async Task InvokeAsync_FinallyExecutes_WhenGenericExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextMiddleware = new RequestDelegate(ctx => throw new Exception("Unexpected error"));
        var middleware = new ExceptionHandlingMiddleware(nextMiddleware, _loggerMock.Object, Mock.Of<IWebHostEnvironment>());

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        AssertLogContains(
            _loggerMock,
            LogLevel.Information,
            $"Completion of request processing. Path: {context.Request.Path}, Method: {context.Request.Method},",
            500
        );
    }
}
