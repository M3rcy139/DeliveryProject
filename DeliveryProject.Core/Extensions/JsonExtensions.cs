using System.Text.Json;

namespace DeliveryProject.Core.Extensions
{
    public static class JsonExtensions
    {
        public static T DeserializeValue<T>(this string json, JsonSerializerOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException("JSON строка не может быть пустой или null.", nameof(json));
            }

            options ??= new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
