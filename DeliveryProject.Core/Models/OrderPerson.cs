namespace DeliveryProject.Core.Models
{
    public class OrderPerson
    {
        public Guid OrderId { get; set; }
        public Guid PersonId { get; set; }

        public Order Order { get; set; }
        public Person Person { get; set; }
    }
}
