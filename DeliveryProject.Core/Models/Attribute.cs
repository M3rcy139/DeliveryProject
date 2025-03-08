using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Models
{
    public class Attribute
    {
        public int Id { get; set; }
        public AttributeKey Key { get; set; }
        public AttributeType Type { get; set; }

        public ICollection<RoleAttribute> RoleAttributes { get; set; }
            = new List<RoleAttribute>();
        public ICollection<AttributeValue> PersonAttributeValues { get; set; }
            = new List<AttributeValue>();
    }
}
