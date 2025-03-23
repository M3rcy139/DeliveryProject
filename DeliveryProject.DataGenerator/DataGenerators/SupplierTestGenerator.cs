using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataGenerator.Helpers;

namespace DeliveryProject.DataGenerator.DataGenerators
{
    public static class SupplierTestGenerator
    {
        public static async Task GenerateSuppliers(this DeliveryDbContext context, int count)
        {
            var suppliers = new List<PersonEntity>();
            var attributes = new List<AttributeValueEntity>();
            var products = new List<ProductEntity>();
            var random = new Random();

            var supplierRole = await context.Roles.FirstAsync(r => r.RoleType == RoleType.Supplier);

            for (int i = 1; i <= count; i++)
            {
                var supplier = new SupplierEntity
                {
                    Id = Guid.NewGuid(),
                    Status = PersonStatus.Active,
                    RegionId = random.Next(1, 80),
                    RoleId = supplierRole.Id,
                    Name = $"Supplier {i}",
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    Rating = Math.Round(4.0 + i % 5 * 0.2, 1),
                    Email = $"supplier{i}@email.com"
                };
                suppliers.Add(supplier);

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

            await TransactionTestHelper.ExecuteInTransactionAsync(context, async () =>
            {
                await context.Persons.AddRangeAsync(suppliers);
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            });
        }
    }
}