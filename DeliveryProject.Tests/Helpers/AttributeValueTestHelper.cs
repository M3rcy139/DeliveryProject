using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Tests.Helpers
{
    public static class AttributeValueTestHelper
    {
        public static AttributeValueEntity CreateAttribute(Guid personId, int attributeId, string value)
        {
            return new AttributeValueEntity
            {
                Id = Guid.NewGuid(),
                PersonId = personId,
                AttributeId = attributeId,
                Value = value
            };
        }
    }
}
