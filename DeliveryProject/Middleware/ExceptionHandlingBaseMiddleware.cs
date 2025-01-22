using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Extensions;
using DeliveryProject.Core.Exceptions;
using System.Net;

namespace DeliveryProject.Middleware
{
    public abstract class ExceptionHandlingBaseMiddleware
    {
        protected readonly RequestDelegate _next;
        protected readonly ILogger<ExceptionHandlingBaseMiddleware> _logger;
        protected readonly IWebHostEnvironment _environment;

        protected ExceptionHandlingBaseMiddleware(RequestDelegate next, ILogger<ExceptionHandlingBaseMiddleware> logger, 
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        protected async Task HandleExceptionResponseAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, 
            string? errorCode = null, IEnumerable<object>? errors = null)
        {
            var problemDetails = new CustomProblemDetails
            {
                Status = (int)statusCode,
                Title = exception.Message,
                Instance = context.Request.Path,
                Type = statusCode.ToString(),
                Detail = !_environment.IsProduction() ? exception.FullMessage() : null,
                Errors = errors
            };

            if (exception is BussinessArgumentException businessException && !string.IsNullOrEmpty(businessException.ErrorCode))
            {
                problemDetails.ErrorCode = businessException.ErrorCode;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
