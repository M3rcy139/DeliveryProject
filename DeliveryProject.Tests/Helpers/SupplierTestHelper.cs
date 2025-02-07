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
                string phone = $"{random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(100, 999)}";
                double rating = Math.Round(4.0 + (i % 5) * 0.2, 1);
                string email = $"supplier{i}@example.com";

                suppliers.Add(new SupplierEntity
                {
                    Id = i,
                    Name = $"Supplier {i}",
                    PhoneNumber = phone,
                    Rating = rating,
                    Email = email
                });
            }

            await context.Suppliers.AddRangeAsync(suppliers);
        }
    }
}