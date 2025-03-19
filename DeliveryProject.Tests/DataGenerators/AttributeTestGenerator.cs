using DeliveryProject.DataAccess;
using DeliveryProject.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Tests.DataGenerators
{
    public static class AttributeTestGenerator
    {
        public static async Task GenerateAttributes(this DeliveryDbContext context)
        {
            if (!context.Attributes.Any())
            {
                var attributes = AttributeTestDataHelper.Attributes;

                await context.Attributes.AddRangeAsync(attributes);
                await context.SaveChangesAsync();  

                var roles = await context.Roles.ToListAsync();

                var roleAttributes = RoleAttributesTestHelper.GetRoleAttributes(roles, attributes.ToList());

                await context.RoleAttributes.AddRangeAsync(roleAttributes);
            }
        }
    }
}
