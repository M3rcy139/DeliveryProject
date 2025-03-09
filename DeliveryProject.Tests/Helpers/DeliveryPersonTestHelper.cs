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
            var deliveryPersons = new List<PersonEntity>();
            var attributes = new List<AttributeValueEntity>();
            var deliverySlots = new List<DeliverySlotEntity>();
            var random = new Random();

            var deliveryRole = await context.Roles.FirstAsync(r => r.RoleType == RoleType.DeliveryPerson);
            var nameAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Name);
            var phoneNumberAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.PhoneNumber);
            var emailAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Email);
            var ratingAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Rating);
            

            for (int i = 1; i <= count; i++)
            {
                var deliveryPerson = new PersonEntity
                {
                    Id = Guid.NewGuid(),
                    Status = PersonStatus.Active,
                    RegionId = random.Next(1, 80),
                    RoleId = deliveryRole.Id
                };

                deliveryPersons.Add(deliveryPerson);

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = deliveryPerson.Id,
                    AttributeId = nameAttribute.Id,
                    Value = $"Delivery Person {i}"
                });

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = deliveryPerson.Id,
                    AttributeId = ratingAttribute.Id,
                    Value = Math.Round(3.5 + (i % 5) * 0.3, 1).ToString()
                });

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = deliveryPerson.Id,
                    AttributeId = phoneNumberAttribute.Id,
                    Value = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}"
                });

                attributes.Add(new AttributeValueEntity
                {
                    Id = Guid.NewGuid(),
                    PersonId = deliveryPerson.Id,
                    AttributeId = emailAttribute.Id,
                    Value = $"deliveryPerson{i}@email.com"
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

            await TransactionHelper.ExecuteInTransactionAsync(context, async () =>
            {
                await context.Persons.AddRangeAsync(deliveryPersons);
                await context.AttributeValues.AddRangeAsync(attributes);
                await context.DeliverySlots.AddRangeAsync(deliverySlots);
                await context.SaveChangesAsync();
            });
        }
    }
}
