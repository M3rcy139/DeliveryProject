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

                var contact = PersonContactHelper.CreatePersonContact(deliveryPerson.Id, random, i, "deliveryperson");
                contacts.Add(contact);

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

            await TransactionHelper.ExecuteInTransactionAsync(context, async () =>
            {
                await context.DeliveryPersons.AddRangeAsync(deliveryPersons);
                await context.PersonContacts.AddRangeAsync(contacts);
                await context.DeliverySlots.AddRangeAsync(deliverySlots);
                await context.SaveChangesAsync();
            });
        }
    }
}
