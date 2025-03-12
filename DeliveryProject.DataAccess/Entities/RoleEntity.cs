using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public RoleType Role { get; set; }
        public ICollection<PersonEntity> Persons { get; set; } = new List<PersonEntity>();
    }
}
