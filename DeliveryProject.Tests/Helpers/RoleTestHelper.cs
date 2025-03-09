using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Tests.Helpers
{
    public static class RoleTestHelper
    {
        public static async Task GenerateRoles(this DeliveryDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<RoleEntity>
                {
                    new RoleEntity { Id = 1, RoleType = RoleType.Customer },
                    new RoleEntity { Id = 2, RoleType = RoleType.Supplier },
                    new RoleEntity { Id = 3, RoleType = RoleType.DeliveryPerson }
                };

                await context.Roles.AddRangeAsync(roles);
            }
        }
    }
}
