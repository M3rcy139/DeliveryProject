using DeliveryProject.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using DeliveryProject.API.Extensions;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DeliveryProject.API.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment; 
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
                        throw new ValidationException("An empty Order object.");
                    }

                    var validationResult = await orderValidator.TryValidateAsync(order);

                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException("Validation error.", validationResult.Errors);
                    }
                }
                catch (ValidationException ex)
                {
                    _logger.LogError("Validation error: {Errors}", ex.Errors);

                    var problemDetails = new CustomProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = ex.Message,
                        Instance = context.Request.Path,
                        Type = HttpStatusCode.BadRequest.ToString(),
                        Detail = !_environment.IsProduction() ? ex.FullMessage() : null, 
                        Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    };

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    await context.Response.WriteAsJsonAsync(problemDetails);
                    return;
                }
            }

            await _next(context);
        }
    }

}
