using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
namespace DeliveryProject.DataGenerator.Generators
{
    internal class DeliveryPersonDataGenerator
    {
        private readonly DeliveryDbContext _context;

        public DeliveryPersonDataGenerator(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task GenerateAndSaveDeliveryPersonsAsync()
        {
            var deliveryPersons = new List<DeliveryPersonEntity>();
            var random = new Random();

            for (int i = 1; i <= 100; i++)
            {
                string phone = $"{(i % 10)}23-456-78{i}";
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
            Console.WriteLine("Delivery persons generated and saved.");
        }
    }
}
