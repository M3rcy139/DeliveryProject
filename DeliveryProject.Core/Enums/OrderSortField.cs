using System.Text.Json.Serialization;

namespace DeliveryProject.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderSortField
    {
        RegionId,
        DeliveryTime
    }
}
