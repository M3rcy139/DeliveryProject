namespace DeliveryProject.Core.Models
{
    public class Supplier : BaseUnit
    {
        public ICollection<Order> OrdersSupplied { get; set; } = new List<Order>();
        public string Email { get; set; }
    }
}
