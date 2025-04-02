using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.DataProviders;

public static class RolesDataProvider
{
    public static List<RoleEntity> GetRoles()
    {
        var roles = Enum
            .GetValues<RoleType>()
            .Select(r => new RoleEntity
            {
                Id = (int)r,
                RoleType = r,
            }).ToList();

        return roles;
    }
}