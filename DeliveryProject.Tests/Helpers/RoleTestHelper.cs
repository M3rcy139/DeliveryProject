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
                    new RoleEntity { Id = 1, Role = RoleType.Supplier },
                    new RoleEntity { Id = 2, Role = RoleType.Customer },
                    new RoleEntity { Id = 3, Role = RoleType.DeliveryPerson }
                };

                await context.Roles.AddRangeAsync(roles);
            }
        }
    }
}
