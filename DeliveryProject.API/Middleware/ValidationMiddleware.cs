using DeliveryProject.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using DeliveryProject.API.Extensions;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using DeliveryProject.Core.Constants;

namespace DeliveryProject.API.Middleware
{
    public class ValidationMiddleware : ExceptionHandlingBaseMiddleware
    {
        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger,
            IWebHostEnvironment environment) : base (next, logger, environment)
        {
        }

        public async Task InvokeAsync(HttpContext context, IValidator<Order> orderValidator)
        {
            if (context.Request.Path.StartsWithSegments("/api/order") &&
                (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put))
            {
                try
                {
                    context.Request.EnableBuffering();

                    string body;
                    using (var reader = new StreamReader(context.Request.Body, leaveOpen: true)) 
                    {
                        body = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0; 
                    }

                    var order = JsonSerializer.Deserialize<Order>(body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (order == null)
                    {
                        throw new ValidationException(ErrorMessages.Validation.EmptyOrderObject);
                    }

                    var isValid = await orderValidator.TryValidateAsync(order);

                    if (isValid)
                    {
                        _logger.LogInformation(InfoMessages.Validation.ValidationSucceeded);
                    }
                }
                catch (ValidationException ex)
                {
                    _logger.LogError(ErrorMessages.Validation.ValidationFailed, ex.Errors);

                    await HandleExceptionResponseAsync(context, ex, HttpStatusCode.BadRequest, errors: ex.Errors);

                    return;
                }
            }

            await _next(context);
        }
    }
}
