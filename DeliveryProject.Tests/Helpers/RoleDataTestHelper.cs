using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Tests.Helpers
{
    public readonly struct RoleDataTestHelper
    {
        public static IReadOnlyList<RoleEntity> Roles { get; } = new List<RoleEntity>
        {
            new RoleEntity { Id = 1, RoleType = RoleType.Customer },
            new RoleEntity { Id = 2, RoleType = RoleType.Supplier },
            new RoleEntity { Id = 3, RoleType = RoleType.DeliveryPerson }
        };
    }
}
