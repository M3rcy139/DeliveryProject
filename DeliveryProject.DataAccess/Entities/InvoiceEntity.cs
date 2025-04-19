namespace DeliveryProject.DataAccess.Entities
{
    public class InvoiceEntity : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid DeliveryPersonId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsExecuted { get; set; }

        public OrderEntity Order { get; set; }
        public PersonEntity DeliveryPerson { get; set; }
    }
}
