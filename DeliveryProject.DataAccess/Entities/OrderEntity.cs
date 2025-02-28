namespace DeliveryProject.DataAccess.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public decimal Amount { get; set; }

        public ICollection<PersonEntity> Persons { get; set; } = new List<PersonEntity>();
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
