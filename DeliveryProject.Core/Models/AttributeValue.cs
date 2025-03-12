namespace DeliveryProject.Core.Models
{
    public class AttributeValue
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid PersonId { get; set; }
        public int AttributeId { get; set; }

        public Person Person { get; set; }
        public Attribute Attribute { get; set; }

    }
}
