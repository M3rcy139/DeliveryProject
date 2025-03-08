namespace DeliveryProject.Core.Models
{
    public class RoleAttribute
    {
        public int RoleId { get; set; }
        public int AttributeId { get; set; }

        public Role Role { get; set; }
        public Attribute Attribute { get; set; }
    }
}
