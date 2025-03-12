using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeliveryProject.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male,
        Female
    }
}
