using DeliveryProject.Core.Models;
using FluentValidation;
using DeliveryProject.Core.Extensions;
using System.Net;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;

namespace DeliveryProject.Middleware
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

                    var order = body.DeserializeValue<Order>();

                    var isValid = await orderValidator.TryValidateAsync(order);

                    if (!isValid)
                    {
                        throw new ValidationException(ValidationErrorMessages.GenericValidationFailed);
                    }

                    _logger.LogInformation(InfoMessages.ValidationSucceeded);
                }
                catch (ValidationException ex)
                {
                    _logger.LogError(ValidationErrorMessages.GenericValidationFailed);

                    await HandleExceptionResponseAsync(context, ex, HttpStatusCode.BadRequest);

                    return;
                }
            }

            await _next(context);
        }
    }
}
