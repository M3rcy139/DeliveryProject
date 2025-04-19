using System.Text;
using Microsoft.AspNetCore.Http;

namespace DeliveryProject.Bussiness.Helpers;

public static class HttpBodyReader
{
    public static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();

        if (request.ContentLength is null or 0)
            return string.Empty;

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        request.Body.Position = 0;

        return Encoding.UTF8.GetString(buffer);
    }
}
