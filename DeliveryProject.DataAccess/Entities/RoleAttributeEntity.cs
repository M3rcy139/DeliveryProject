namespace DeliveryProject.DataAccess.Entities
{
    public class RoleAttributeEntity
    {
        public int RoleId { get; set; }
        public int AttributeId { get; set; }

        public RoleEntity Role { get; set; }
        public AttributeEntity Attribute { get; set; }
    }
}
