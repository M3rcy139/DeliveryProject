using DeliveryProject.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using DeliveryProject.API.Extensions;

namespace DeliveryProject.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogError("Ошибка валидации: {Errors}", ex.Errors);

                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (BussinessArgumentException ex)
            {
                _logger.LogError($"Бизнес-логическое исключение: {ex.Message}");

                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Аргументное исключение: {ex.Message}");

                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Непредвиденная ошибка: {ex.Message}");

                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.LogInformation("Завершение обработки запроса. Path: {Path}, Method: {Method}, StatusCode: {StatusCode}",
                    context.Request.Path, context.Request.Method, context.Response.StatusCode);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exp, HttpStatusCode code)
        {
            var problemDetails = new CustomProblemDetails
            {
                Status = (int)code,
                Title = exp.Message,
                Instance = context.Request.Path,
                Type = code.ToString(),
                Detail = !_environment.IsProduction() ? exp.FullMessage() : null
            };

            if (exp is BussinessArgumentException businessException && !string.IsNullOrEmpty(businessException.ErrorCode))
            {
                problemDetails.ErrorCode = businessException.ErrorCode;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
