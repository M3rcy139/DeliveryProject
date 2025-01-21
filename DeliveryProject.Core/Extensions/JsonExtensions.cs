using DeliveryProject.Core.Constants.ErrorMessages;
using System.Text.Json;

namespace DeliveryProject.Core.Extensions
{
    public static class JsonExtensions
    {
        public static T DeserializeValue<T>(this string json, JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(ErrorMessages.JsonEmpty, nameof(json));
            }

            options ??= new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
