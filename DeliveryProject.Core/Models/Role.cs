using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public RoleType RoleType { get; set; }
        public ICollection<Person> Persons { get; set; } = new List<Person>();
    }
}
