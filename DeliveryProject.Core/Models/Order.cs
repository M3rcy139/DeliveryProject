namespace DeliveryProject.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; } 
        public DateTime DeliveryTime { get; set; } 
        public decimal Amount { get; set; }

        public ICollection<Person> Persons { get; set; } = new List<Person>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>(); 
    }
}

