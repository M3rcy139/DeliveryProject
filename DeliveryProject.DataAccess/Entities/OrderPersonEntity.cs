namespace DeliveryProject.DataAccess.Entities
{
    public class OrderPersonEntity
    {
        public Guid OrderId { get; set; }
        public Guid PersonId { get; set; }

        public OrderEntity Order { get; set; }
        public PersonEntity Person { get; set; }
    }
}
