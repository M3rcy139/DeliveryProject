namespace DeliveryProject.Core.Models
{
    public class FilteredOrder
    {
        public Guid Id { get; set; } 
        public Guid OrderId { get; set; }

        public Order Order { get; set; }
    }
}
