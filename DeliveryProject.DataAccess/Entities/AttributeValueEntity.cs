namespace DeliveryProject.DataAccess.Entities
{
    public class AttributeValueEntity
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid PersonId { get; set; }
        public int AttributeId { get; set; }

        public PersonEntity Person { get; set; }
        public AttributeEntity Attribute { get; set; }

    }
}
