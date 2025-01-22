using System.Text.Json.Serialization;

namespace DeliveryProject.Bussiness.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderSortField
    {
        Weight,
        RegionId,
        DeliveryTime
    }
}
