using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using DeliveryProject.Bussiness.Helpers;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;

namespace DeliveryProject.Middleware
{
    public class CentralizedHttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CentralizedHttpLoggingMiddleware> _logger;

        public CentralizedHttpLoggingMiddleware(RequestDelegate next, ILogger<CentralizedHttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;

            var stopwatch = Stopwatch.StartNew();

            string requestBody = await HttpBodyReader.ReadRequestBodyAsync(context.Request);
            _logger.LogInformation(InfoMessages.HttpRequest,
                requestId,
                context.Request.Method,
                context.Request.Path,
                BodyTruncator.Truncate(requestBody));

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

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(InfoMessages.HttpResponse,
                requestId,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                BodyTruncator.Truncate(responseText));

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
