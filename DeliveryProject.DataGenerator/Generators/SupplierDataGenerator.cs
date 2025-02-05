using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.DataGenerator.Generators
{
    internal class SupplierDataGenerator
    {
        private readonly DeliveryDbContext _context;

        public SupplierDataGenerator(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task GenerateAndSaveSuppliersAsync()
        {
            var suppliers = new List<SupplierEntity>();
            var random = new Random();

            for (int i = 1; i <= 50; i++)
            {
                string phone = $"{(i % 10)}123-456-789{i}";
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
            Console.WriteLine("Suppliers generated and saved.");
        }
    }
}
