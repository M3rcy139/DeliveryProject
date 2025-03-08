using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Entities
{
    public class PersonEntity
    {
        public Guid Id { get; set; }
        public PersonStatus Status { get; set; } 
        public int RegionId { get; set; }
        public int RoleId { get; set; }

        public RegionEntity Region { get; set; }
        public RoleEntity Role { get; set; }
        public ICollection<AttributeValueEntity> PersonAttributeValues { get; set; } 
            = new List<AttributeValueEntity>();
        public ICollection<OrderPersonEntity> OrderPersons { get; set; } = new List<OrderPersonEntity>();
    }
}