namespace DeliveryProject.DataAccess.Entities
{
    public class InvoiceEntity
    {
        public Guid Id { get; set; }
        
        public Guid OrderId { get; set; }
        public Guid DeliveryPersonId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsExecuted { get; set; }

        public OrderEntity Order { get; set; }
        public PersonEntity DeliveryPerson { get; set; }
    }
}
