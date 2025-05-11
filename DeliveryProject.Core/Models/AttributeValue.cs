namespace DeliveryProject.Core.Models
{
    public class AttributeValue : BaseModel
    {
        public string Value { get; set; }
        public Guid PersonId { get; set; }
        public int AttributeId { get; set; }

        public Person Person { get; set; }
        public Attribute Attribute { get; set; }
    }
}
