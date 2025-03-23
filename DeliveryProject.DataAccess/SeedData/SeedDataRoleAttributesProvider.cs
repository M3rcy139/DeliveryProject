using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.SeedData;

public static class SeedDataRoleAttributesProvider
{
    public static List<RoleAttributeEntity> GetRoleAttributes()
    {
        var roleAttributes = new List<RoleAttributeEntity>();

        foreach (var role in Enum.GetValues<RoleType>())
        {
            roleAttributes.AddRange([
                new RoleAttributeEntity { RoleId = (int)role, AttributeId = (int)AttributeKey.Name },
                new RoleAttributeEntity { RoleId = (int)role, AttributeId = (int)AttributeKey.PhoneNumber },
                new RoleAttributeEntity { RoleId = (int)role, AttributeId = (int)AttributeKey.Email }
            ]);

            if (role is RoleType.DeliveryPerson or RoleType.Supplier)
            {
                roleAttributes.Add(new RoleAttributeEntity
                {
                    RoleId = (int)role,
                    AttributeId = (int)AttributeKey.Rating
                });
            }

            if (role == RoleType.Customer)
            {
                roleAttributes.AddRange([
                    new RoleAttributeEntity { RoleId = (int)role, AttributeId = (int)AttributeKey.LastName },
                    new RoleAttributeEntity { RoleId = (int)role, AttributeId = (int)AttributeKey.Sex }
                ]);
            }
        }

        return roleAttributes;
    }
}