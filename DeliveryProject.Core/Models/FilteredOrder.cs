namespace DeliveryProject.Core.Models
{
    public class FilteredOrder
    {
        public Guid Id { get; set; } 
        public Guid OrderId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public decimal Amount { get; set; }

        public Order Order { get; set; }
        public ICollection<Person> Persons { get; set; } = new List<Person>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
