using System.Diagnostics;
using DeliveryProject.Business.Helpers;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;
using Microsoft.AspNetCore.Identity.Data;

namespace DeliveryProject.Middleware
{
    public class HttpLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpLoggerMiddleware> _logger;

        public HttpLoggerMiddleware(RequestDelegate next, ILogger<HttpLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;

            var stopwatch = Stopwatch.StartNew();

            await HttpLoggingHelper.LogRequestAsync(context, _logger, requestId);

            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                _logger.LogError(ex, ErrorMessages.UnhandledExceptionDuringHttpRequest, requestId);
                await context.Response.WriteAsync(ErrorMessages.UnexpectedError);
            }

            stopwatch.Stop();

            await HttpLoggingHelper.LogResponseAsync(context, _logger, requestId, stopwatch.ElapsedMilliseconds);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
