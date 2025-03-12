namespace DeliveryProject.Core.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public Guid OrderId { get; set; }
        public Guid DeliveryPersonId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsExecuted { get; set; }

        public Order Order { get; set; }
        public Person DeliveryPerson { get; set; }
    }
}
