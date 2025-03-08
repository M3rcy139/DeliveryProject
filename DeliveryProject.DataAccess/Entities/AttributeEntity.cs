using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Entities
{
    public class AttributeEntity
    {
        public int Id { get; set; }
        public AttributeKey Key { get; set; } 
        public AttributeType Type { get; set; } 

        public ICollection<RoleAttributeEntity> RoleAttributes { get; set; } 
            = new List<RoleAttributeEntity>();
        public ICollection<AttributeValueEntity> AttributeValues { get; set; } 
            = new List<AttributeValueEntity>();
    }
}
