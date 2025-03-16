using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Core.Extensions
{
    public static class PersonExtensions
    {
        public static string? GetAttributeValue(this Person person, AttributeKey key)
        {
            return person.AttributeValues
                .FirstOrDefault(av => av.Attribute.Key == key)?
                .Value;
        }

        public static void SetAttributeValue(this Person person, AttributeKey key, string? value)
        {
            var attributeValue = person.AttributeValues.FirstOrDefault(av => av.Attribute != null && av.Attribute.Key == key);

            if (attributeValue != null)
            {
                attributeValue.Value = value;
            }
            else
            {
                person.AttributeValues.Add(new AttributeValue
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
