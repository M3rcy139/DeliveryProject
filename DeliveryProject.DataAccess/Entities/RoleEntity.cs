using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public RoleType RoleType { get; set; }
        public ICollection<RoleAttributeEntity> RoleAttributes { get; set; } 
            = new List<RoleAttributeEntity>();
    }
}
