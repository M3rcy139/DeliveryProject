using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.Tets.Helpers
{
    internal class SupplierTestHelper
    {
        private readonly DeliveryDbContext _context;
        private readonly int _suppliersCount;

        public SupplierTestHelper(DeliveryDbContext context, int suppliersCount)
        {
            _context = context;
            _suppliersCount = suppliersCount; 
        }

        public async Task GenerateAndSaveSuppliersAsync()
        {
            var suppliers = new List<SupplierEntity>();
            var random = new Random();

            for (int i = 1; i <= _suppliersCount; i++)
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

            await _context.Suppliers.AddRangeAsync(suppliers);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{_suppliersCount} suppliers generated and saved.");
        }
    }
}
