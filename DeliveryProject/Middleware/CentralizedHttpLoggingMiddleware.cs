using Serilog;
using System.Diagnostics;
using System.Text;


namespace DeliveryProject.Middleware
{
    public class CentralizedHttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public CentralizedHttpLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;

            var stopwatch = Stopwatch.StartNew();

            var requestBody = await ReadRequestBodyAsync(context.Request);
            var requestLog = new
            {
                Type = "Request",
                RequestId = requestId,
                Timestamp = DateTime.UtcNow,
                Method = context.Request.Method,
                Url = context.Request.Path,
                Headers = context.Request.Headers.ToDictionary(k => k.Key, v => v.Value.ToString()),
                Body = requestBody
            };

            Log.Information("HTTP Request: {@Request}", requestLog);

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

                Log.Error(ex, "Unhandled exception occurred during HTTP request: {RequestId}", requestId);

                await context.Response.WriteAsync("An unexpected error occurred.");
            }

            stopwatch.Stop();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseLog = new
            {
                Type = "Response",
                RequestId = requestId,
                Timestamp = DateTime.UtcNow,
                StatusCode = context.Response.StatusCode,
                Duration = stopwatch.ElapsedMilliseconds,
                Headers = context.Response.Headers.ToDictionary(k => k.Key, v => v.Value.ToString()),
                Body = responseBodyText
            };

            Log.Information("HTTP Response: {@Response}", responseLog);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength ?? 0)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            request.Body.Position = 0;

            return Encoding.UTF8.GetString(buffer);
        }
    }
}
