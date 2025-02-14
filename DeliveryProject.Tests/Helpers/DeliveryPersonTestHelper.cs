using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.Tests.Helpers
{
    public static class DeliveryPersonTestHelper
    {
        public static async Task GenerateDeliveryPersons(this DeliveryDbContext context, int count)
        {
            var deliveryPersons = new List<DeliveryPersonEntity>();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                deliveryPersons.Add(new DeliveryPersonEntity
                {
                    Id = i,
                    Name = $"Delivery Person {i}",
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(100, 999)}",
                    Rating = Math.Round(3.5 + (i % 5) * 0.3, 1),
                    DeliverySlots = new List<DateTime>()
                });
            }

            await context.DeliveryPersons.AddRangeAsync(deliveryPersons);
        }
    }
}
