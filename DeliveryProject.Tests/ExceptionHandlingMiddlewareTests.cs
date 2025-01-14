using DeliveryProject.API.Middleware;
using DeliveryProject.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using FluentValidation;
using Xunit;
using System.Net;

namespace DeliveryProject.Tests
{
    public class ExceptionHandlingMiddlewareTests
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
            var nextMiddleware = new RequestDelegate(async (ctx) => await Task.CompletedTask); // Успешный вызов следующего middleware
            var middleware = new ExceptionHandlingMiddleware(nextMiddleware, _loggerMock.Object, Mock.Of<IWebHostEnvironment>());

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()
                        .Contains("Completion of request processing. Path: , Method: , StatusCode: 200")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once); 
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
            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()
                        .Contains($"Completion of request processing. Path: {context.Request.Path}, " +
                            $"Method: {context.Request.Method}, StatusCode: 400")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once); 
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
            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()
                        .Contains($"Completion of request processing. Path: {context.Request.Path}, " +
                            $"Method: {context.Request.Method}, StatusCode: 500")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once); 
        }

    }
}
