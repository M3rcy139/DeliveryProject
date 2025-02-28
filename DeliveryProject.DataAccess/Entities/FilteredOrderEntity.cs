namespace DeliveryProject.DataAccess.Entities
{
    public class FilteredOrderEntity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public decimal Amount { get; set; }

        public OrderEntity Order { get; set; }
        public ICollection<PersonEntity> Persons { get; set; } = new List<PersonEntity>();
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
