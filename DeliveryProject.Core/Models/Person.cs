namespace DeliveryProject.Core.Models
{
    public abstract class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<PersonContact> Contacts { get; set; } = new List<PersonContact>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
