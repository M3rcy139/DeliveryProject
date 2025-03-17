using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;


namespace DeliveryProject.Core.Models
{
    public abstract class Person
    {
        public Guid Id { get; set; }
        public PersonStatus Status { get; set; }
        public int RegionId { get; set; }
        public int RoleId { get; set; }

        public Region Region { get; set; }
        protected virtual Role Role { get; set; }
        protected virtual ICollection<AttributeValue> AttributeValues { get; set; }
            = new List<AttributeValue>();

        [NotMapped]
        public string? Name
        {
            get => GetAttributeValue(AttributeKey.Name);
            set => SetAttributeValue(AttributeKey.Name, value);
        }

        [NotMapped]
        public string? PhoneNumber
        {
            get => GetAttributeValue(AttributeKey.PhoneNumber);
            set => SetAttributeValue(AttributeKey.PhoneNumber, value);
        }

        [NotMapped]
        public string? Email
        {
            get => GetAttributeValue(AttributeKey.Email);
            set => SetAttributeValue(AttributeKey.Email, value);
        }

        protected string? GetAttributeValue(AttributeKey key)
        {
            return AttributeValues
                .FirstOrDefault(av => av.Attribute?.Key == key)?
                .Value;
        }

        protected void SetAttributeValue(AttributeKey key, string? value)
        {
            var attributeValue = AttributeValues.FirstOrDefault(av => av.Attribute != null && av.Attribute.Key == key);

            if (attributeValue != null)
            {
                attributeValue.Value = value;
            }
            else
            {
                AttributeValues.Add(new AttributeValue
                {
                    Id = Guid.NewGuid(),
                    PersonId = Id,
                    AttributeId = (int)key,
                    Value = value,
                });
            }

        }
    }
}
