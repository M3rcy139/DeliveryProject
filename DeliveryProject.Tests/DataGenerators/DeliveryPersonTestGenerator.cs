using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;
using DeliveryProject.Tests.Helpers;

namespace DeliveryProject.Tests.DataGenerators
{
    public static class DeliveryPersonTestGenerator
    {
        public static async Task GenerateDeliveryPersons(this DeliveryDbContext context, int count)
        {
            var deliveryPersons = new List<PersonEntity>();
            var attributes = new List<AttributeValueEntity>();
            var deliverySlots = new List<DeliverySlotEntity>();
            var random = new Random();

            var deliveryRole = await context.Roles.FirstAsync(r => r.RoleType == RoleType.DeliveryPerson);

            for (int i = 1; i <= count; i++)
            {
                var deliveryPerson = new DeliveryPersonEntity
                {
                    Id = Guid.NewGuid(),
                    Status = PersonStatus.Active,
                    RegionId = random.Next(1, 80),
                    RoleId = deliveryRole.Id,
                    Name = $"Delivery Person {i}",
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    Rating = Math.Round(4.0 + i % 5 * 0.2, 1),
                    Email = $"deliveryPerson{i}@email.com"
                };

                deliveryPersons.Add(deliveryPerson);

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

            await TransactionTestHelper.ExecuteInTransactionAsync(context, async () =>
            {
                await context.Persons.AddRangeAsync(deliveryPersons);
                await context.DeliverySlots.AddRangeAsync(deliverySlots);
                await context.SaveChangesAsync();
            });
        }
    }
}
