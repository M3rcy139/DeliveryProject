using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public RoleType RoleType { get; set; }
        public ICollection<RoleAttribute> RoleAttributes { get; set; }
            = new List<RoleAttribute>();
    }
}
