using System.Text.Json.Serialization;

namespace DeliveryProject.DataAccess.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UploadType
    {
        DeliveryPerson,
        Supplier
    }
}
