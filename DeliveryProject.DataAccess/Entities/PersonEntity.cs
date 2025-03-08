namespace DeliveryProject.DataAccess.Entities
{
    public abstract class PersonEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<PersonContactEntity> Contacts { get; set; } = new List<PersonContactEntity>();
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }
}
