using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.Tests.Helpers
{
    public static class SupplierTestHelper
    {
        public static async Task GenerateSuppliers(this DeliveryDbContext context, int count)
        {
            var suppliers = new List<SupplierEntity>();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                suppliers.Add(new SupplierEntity
                {
                    Id = i,
                    Name = $"Supplier {i}",
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(100, 999)}",
                    Rating = Math.Round(4.0 + (i % 5) * 0.2, 1),
                    Email = $"supplier{i}@example.com"
                });
            }

            await context.Suppliers.AddRangeAsync(suppliers);
        }
    }
}