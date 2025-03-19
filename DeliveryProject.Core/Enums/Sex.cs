using System.Text.Json.Serialization;

namespace DeliveryProject.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Sex
    {
        Male,
        Female
    }
}
