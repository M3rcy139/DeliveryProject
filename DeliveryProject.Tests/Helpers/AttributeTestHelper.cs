using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Tests.Helpers
{
    public static class AttributeTestHelper
    {
        public static async Task GenerateAttributes(this DeliveryDbContext context)
        {
            if (!context.Attributes.Any())
            {
                var attributes = new List<AttributeEntity>
                { 
                    new AttributeEntity { Id = 1, Key = AttributeKey.Name, Type = AttributeType.String },
                    new AttributeEntity { Id = 2, Key = AttributeKey.LastName, Type = AttributeType.String },
                    new AttributeEntity { Id = 3, Key = AttributeKey.Sex, Type = AttributeType.String },
                    new AttributeEntity { Id = 4, Key = AttributeKey.PhoneNumber, Type = AttributeType.String },
                    new AttributeEntity { Id = 5, Key = AttributeKey.Email, Type = AttributeType.String },
                    new AttributeEntity { Id = 6, Key = AttributeKey.Rating, Type = AttributeType.Double }
                };

                await context.Attributes.AddRangeAsync(attributes);
                await context.SaveChangesAsync();  

                var roles = await context.Roles.ToListAsync();

                var roleAttributes = new List<RoleAttributeEntity>();

                foreach (var role in roles)
                {
                    roleAttributes.Add(new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes
                        .First(a => a.Key == AttributeKey.Name).Id });
                    roleAttributes.Add(new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes
                        .First(a => a.Key == AttributeKey.PhoneNumber).Id });
                    roleAttributes.Add(new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes
                        .First(a => a.Key == AttributeKey.Email).Id });

                    if (role.RoleType == RoleType.DeliveryPerson || role.RoleType == RoleType.Supplier)
                    {
                        roleAttributes.Add(new RoleAttributeEntity { RoleId = role.Id, AttributeId = attributes
                            .First(a => a.Key == AttributeKey.Rating).Id });
                    }

                    if (role.RoleType == RoleType.Customer)
                    {
                        roleAttributes.Add(new RoleAttributeEntity
                        { RoleId = role.Id, AttributeId = attributes
                            .First(a => a.Key == AttributeKey.LastName).Id });
                        
                        roleAttributes.Add(new RoleAttributeEntity
                        { RoleId = role.Id, AttributeId = attributes
                            .First(a => a.Key == AttributeKey.Sex).Id });
                    }
                }

                await context.RoleAttributes.AddRangeAsync(roleAttributes);
            }
        }
    }
}
