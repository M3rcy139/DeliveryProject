using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.SeedData;

public static class SeedDataRolesProvider
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