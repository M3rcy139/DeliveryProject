using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.Tests.Helpers
{
    public static class DeliveryPersonTestHelper
    {
        public static async Task GenerateDeliveryPersons(this DeliveryDbContext context, int count)
        {
            var deliveryPersons = new List<DeliveryPersonEntity>();
            var contacts = new List<PersonContactEntity>();
            var deliverySlots = new List<DeliverySlotEntity>();
            var random = new Random();

            var deliveryRole = await context.Roles.FirstAsync(r => r.Role == RoleType.DeliveryPerson);

            for (int i = 1; i <= count; i++)
            {
                var deliveryPerson = new DeliveryPersonEntity
                {
                    Id = Guid.NewGuid(), 
                    Name = $"Delivery Person {i}",
                    Rating = Math.Round(3.5 + (i % 5) * 0.3, 1),
                    RoleId = deliveryRole.Id
                };

                deliveryPersons.Add(deliveryPerson);

                contacts.Add(new PersonContactEntity
                {
                    Id = Guid.NewGuid(),
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    Email = $"deliveryperson{i}@example.com",
                    RegionId = random.Next(1, 80),
                    PersonId = deliveryPerson.Id
                });

                int slotCount = random.Next(1, 6);
                for (int j = 0; j < slotCount; j++)
                {
                    deliverySlots.Add(new DeliverySlotEntity
                    {
                        Id = Guid.NewGuid(),
                        SlotTime = DateTime.UtcNow.AddMonths(10).AddDays(random.Next(0, 7)).AddHours(random.Next(8, 20)),
                        DeliveryPersonId = deliveryPerson.Id
                    });
                }
            }

            await context.DeliveryPersons.AddRangeAsync(deliveryPersons);
            await context.SaveChangesAsync();

            await context.PersonContacts.AddRangeAsync(contacts);
            await context.SaveChangesAsync();

            await context.DeliverySlots.AddRangeAsync(deliverySlots);
            await context.SaveChangesAsync();
        }
    }
}
