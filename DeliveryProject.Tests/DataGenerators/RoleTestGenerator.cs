using DeliveryProject.DataAccess;
using DeliveryProject.Tests.Helpers;

namespace DeliveryProject.Tests.DataGenerators
{
    public static class RoleTestGenerator
    {
        public static async Task GenerateRoles(this DeliveryDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = RoleDataTestHelper.Roles;

                await context.Roles.AddRangeAsync(roles);
            }
        }
    }
}
