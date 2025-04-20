using DeliveryProject.Core.Constants.InfoMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.Business.Helpers
{
    public static class HttpLoggingHelper
    {
        public static async Task LogRequestAsync(HttpContext context, ILogger logger, string requestId)
        {
            var requestBody = await HttpBodyReader.ReadRequestBodyAsync(context.Request);
            logger.LogInformation(InfoMessages.HttpRequest,
                requestId,
                context.Request.Method,
                context.Request.Path,
                BodyTruncator.Truncate(requestBody));
        }

        public static async Task LogResponseAsync(HttpContext context, ILogger logger, string requestId, long duration)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            logger.LogInformation(InfoMessages.HttpResponse,
                requestId,
                context.Response.StatusCode,
                duration,
                BodyTruncator.Truncate(responseBody));
        }
    }
}
