using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public PersonStatus Status { get; set; }
        public int RegionId { get; set; }
        public int RoleId { get; set; }

        public Region Region { get; set; }
        public Role Role { get; set; }
        public ICollection<AttributeValue> AttributeValues { get; set; }
            = new List<AttributeValue>();
    }
}
