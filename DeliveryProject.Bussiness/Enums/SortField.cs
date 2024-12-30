using System.Text.Json.Serialization;

namespace DeliveryProject.Bussiness.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortField
    {
        Weight,
        RegionId,
        DeliveryTime
    }
}
