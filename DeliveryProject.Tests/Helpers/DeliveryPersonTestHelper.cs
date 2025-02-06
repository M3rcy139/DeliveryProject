using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.Tets.Helpers
{
    internal class DeliveryPersonTestHelper
    {
        private readonly DeliveryDbContext _context;
        private readonly int _deliveryPersonsCount;

        public DeliveryPersonTestHelper(DeliveryDbContext context, int deliveryPersonsCount)
        {
            _context = context;
            _deliveryPersonsCount = deliveryPersonsCount;
        }

        public async Task GenerateAndSaveDeliveryPersonsAsync()
        {
            var deliveryPersons = new List<DeliveryPersonEntity>();
            var random = new Random();

            for (int i = 1; i <= _deliveryPersonsCount; i++)
            {
                string phone = $"{random.Next(100, 999)}-{random.Next(100,999)}-{random.Next(100,999)}";
                double rating = Math.Round(3.5 + (i % 5) * 0.3, 1);

                deliveryPersons.Add(new DeliveryPersonEntity
                {
                    Id = i,
                    Name = $"Delivery Person {i}",
                    PhoneNumber = phone,
                    Rating = rating,
                    DeliverySlots = new List<DateTime>()
                });
            }

            await _context.DeliveryPersons.AddRangeAsync(deliveryPersons);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{_deliveryPersonsCount} delivery persons generated and saved.");
        }
    }
}
