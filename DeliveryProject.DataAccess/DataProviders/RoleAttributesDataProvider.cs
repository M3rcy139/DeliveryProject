using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.DataProviders;

public static class RoleAttributesDataProvider
{
    public static List<RoleAttributeEntity> GetRoleAttributes()
    {
        var roleAttributes = new List<RoleAttributeEntity>();

        foreach (var role in Enum.GetValues<RoleType>())
        {
            roleAttributes.AddRange(GetBaseAttributes((int)role));
            
            switch (role)
            {
                case RoleType.DeliveryPerson:
                case RoleType.Supplier:
                    roleAttributes.AddRange(GetRatingAttributes((int)role));
                    break;
                case RoleType.Customer:
                    roleAttributes.AddRange(GetCustomerAttributes((int)role));
                    break;
            }
        }

        return roleAttributes;
    }

    private static List<RoleAttributeEntity> GetBaseAttributes(int roleId)
    {
        return
        [
            new RoleAttributeEntity { RoleId = roleId, AttributeId = (int)AttributeKey.Name },
            new RoleAttributeEntity { RoleId = roleId, AttributeId = (int)AttributeKey.PhoneNumber },
            new RoleAttributeEntity { RoleId = roleId, AttributeId = (int)AttributeKey.Email }
        ];
    }

    private static List<RoleAttributeEntity> GetRatingAttributes(int roleId)
    {
        return
        [
            new RoleAttributeEntity { RoleId = roleId, AttributeId = (int)AttributeKey.Rating }
        ];
    }

    private static List<RoleAttributeEntity> GetCustomerAttributes(int roleId)
    {
        return
        [
            new RoleAttributeEntity { RoleId = roleId, AttributeId = (int)AttributeKey.LastName },
            new RoleAttributeEntity { RoleId = roleId, AttributeId = (int)AttributeKey.Sex }
        ];
    }
}