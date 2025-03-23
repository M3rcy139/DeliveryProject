using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataGenerator.Helpers
{
    public static class RoleAttributesTestHelper
    {
        public static List<RoleAttributeEntity> GetRoleAttributes(List<RoleEntity> roles, List<AttributeEntity> attributes)
        {
            var roleAttributes = new List<RoleAttributeEntity>();

            foreach (var role in roles)
            {
                roleAttributes.AddRange(new[]
                {
                new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes.First(a => a.Key == AttributeKey.Name).Id },
                new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes.First(a => a.Key == AttributeKey.PhoneNumber).Id },
                new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes.First(a => a.Key == AttributeKey.Email).Id }
            });

                if (role.RoleType is RoleType.DeliveryPerson or RoleType.Supplier)
                {
                    roleAttributes.Add(new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes
                        .First(a => a.Key == AttributeKey.Rating).Id });
                }

                if (role.RoleType == RoleType.Customer)
                {
                    roleAttributes.AddRange(new[]
                    {
                    new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes.First(a => a.Key == AttributeKey.LastName).Id },
                    new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes.First(a => a.Key == AttributeKey.Sex).Id }
                });
                }
            }

            return roleAttributes;
        }
    }
}
