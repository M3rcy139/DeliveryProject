namespace DeliveryProject.DataAccess.Entities
{
    public class TempAttributeValue : BaseEntity
    {
        public Guid DeliveryPersonId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }

        public TempDeliveryPerson DeliveryPerson { get; set; }  
    }
}
