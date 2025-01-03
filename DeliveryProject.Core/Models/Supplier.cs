namespace DeliveryProject.Core.Models
{
    public class Supplier : Participant
    {
        public ICollection<Order> OrdersSupplied { get; set; } = new List<Order>();
        public string Email { get; set; }
    }
}
