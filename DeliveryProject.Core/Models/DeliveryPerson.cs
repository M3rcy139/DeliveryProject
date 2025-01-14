namespace DeliveryProject.Core.Models
{
    public class DeliveryPerson : BaseUnit
    {
        public ICollection<Order> OrdersDelivered { get; set; } = new List<Order>();
        public List<DateTime> DeliverySlots { get; set; } = new();
    }
}
