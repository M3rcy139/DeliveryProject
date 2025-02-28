using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.Tests.Helpers
{
    public static class SupplierTestHelper
    {
        public static async Task GenerateSuppliers(this DeliveryDbContext context, int count)
        {
            var suppliers = new List<SupplierEntity>();
            var contacts = new List<PersonContactEntity>();
            var products = new List<ProductEntity>();
            var random = new Random();

            var supplierRole = await context.Roles.FirstAsync(r => r.Role == RoleType.Supplier);

            for (int i = 1; i <= count; i++)
            {
                var supplier = new SupplierEntity
                {
                    Id = Guid.NewGuid(), 
                    Name = $"Supplier {i}",
                    Rating = Math.Round(4.0 + (i % 5) * 0.2, 1),
                    RoleId = supplierRole.Id
                };

                suppliers.Add(supplier);

                contacts.Add(new PersonContactEntity
                {
                    Id = Guid.NewGuid(), 
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    Email = $"supplier{i}@example.com",
                    RegionId = random.Next(1, 80),
                    PersonId = supplier.Id  
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

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();

            await context.PersonContacts.AddRangeAsync(contacts);
            await context.SaveChangesAsync();

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}