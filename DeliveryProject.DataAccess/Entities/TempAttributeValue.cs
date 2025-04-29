namespace DeliveryProject.DataAccess.Entities
{
    public class TempAttributeValue
    {
        public Guid Id { get; set; }
        public Guid DeliveryPersonId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }

        public TempDeliveryPerson DeliveryPerson { get; set; }  
    }
}
