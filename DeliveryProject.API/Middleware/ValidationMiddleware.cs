using DeliveryProject.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using DeliveryProject.API.Extensions;

namespace DeliveryProject.API.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IValidator<Order> orderValidator)
        {
            if (context.Request.Path.StartsWithSegments("/api/order") &&
                (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put))
            {
                try
                {
                    context.Request.EnableBuffering();
                    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    var order = JsonSerializer.Deserialize<Order>(body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (order == null)
                    {
                        throw new ValidationException("Пустой объект Order.");
                    }

                    var validationResult = await orderValidator.TryValidateAsync(order);

                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException("Ошибка валидации.", validationResult.Errors);
                    }
                }
                catch (ValidationException ex)
                {
                    _logger.LogError("Ошибка валидации: {Errors}", ex.Errors);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = ex.Message,
                        Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    });
                    return;
                }
            }

            await _next(context);
        }
    }

}
