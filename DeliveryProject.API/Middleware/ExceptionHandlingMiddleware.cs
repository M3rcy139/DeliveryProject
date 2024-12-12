using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Аргументное исключение: {ex.Message}");

                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsJsonAsync(new { error = "Данные не найдены", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Непредвиденная ошибка: {ex.Message}");
                
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                
                await context.Response.WriteAsJsonAsync(new { error = "Произошла ошибка непредвиденная ошибка", details = ex.Message });
            }
        }
    }

}
