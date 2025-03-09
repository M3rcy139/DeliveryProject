using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Tests.Helpers
{
    public static class SupplierTestHelper
    {
        public static async Task GenerateSuppliers(this DeliveryDbContext context, int count)
        {
            var suppliers = new List<PersonEntity>();
            var attributes = new List<AttributeValueEntity>();
            var products = new List<ProductEntity>();
            var random = new Random();

            var supplierRole = await context.Roles.FirstAsync(r => r.RoleType == RoleType.Supplier);
            var nameAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Name);
            var phoneNumberAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.PhoneNumber);
            var emailAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Email);
            var ratingAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Rating);

            for (int i = 1; i <= count; i++)
            {
                var supplier = new PersonEntity
                {
                    Id = Guid.NewGuid(),
                    Status = PersonStatus.Active,
                    RegionId = random.Next(1, 80),
                    RoleId = supplierRole.Id
                };
                suppliers.Add(supplier);

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = supplier.Id,
                    AttributeId = nameAttribute.Id,
                    Value = $"Supplier {i}"
                });

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = supplier.Id,
                    AttributeId = ratingAttribute.Id,
                    Value = Math.Round(4.0 + (i % 5) * 0.2, 1).ToString()
                });

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = supplier.Id,
                    AttributeId = phoneNumberAttribute.Id,
                    Value = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}"
                });

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = supplier.Id,
                    AttributeId = emailAttribute.Id,
                    Value = $"supplier{i}@email.com"
                });

                int productCount = random.Next(3, 11);
                for (int j = 1; j <= productCount; j++)
                {
                    products.Add(new ProductEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Product {i}-{j}",
                        Weight = Math.Round(random.NextDouble() * 10, 2),
                        Price = Math.Round((decimal)(random.NextDouble() * 300), 2),
                        SupplierId = supplier.Id
                    });
                }
            }

            await TransactionHelper.ExecuteInTransactionAsync(context, async () =>
            {
                await context.Persons.AddRangeAsync(suppliers);
                await context.AttributeValues.AddRangeAsync(attributes);
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            });
        }
    }
}