using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Extensions
{
    public static class PersonEntityExtensions
    {
        public static string? GetAttributeValue(this PersonEntity person, AttributeKey key)
        {
            return person.AttributeValues
                .FirstOrDefault(av => av.Attribute.Key == key)?
                .Value;
        }

        public static void SetAttributeValue(this PersonEntity person, AttributeKey key, string? value)
        {
            var attributeValue = person.AttributeValues.FirstOrDefault(av => av.Attribute != null && av.Attribute.Key == key);

            if (attributeValue != null)
            {
                attributeValue.Value = value;
            }
            else
            {
                person.AttributeValues.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = person.Id,
                    AttributeId = (int)key,
                    Value = value,
                });
            }

        }
    }
}
