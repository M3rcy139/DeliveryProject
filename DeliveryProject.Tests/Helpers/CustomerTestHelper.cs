using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Models;
using CsvHelper.Configuration.Attributes;

namespace DeliveryProject.Tests.Helpers
{
    public static class CustomerTestHelper
    {
        public static async Task GenerateCustomers(this DeliveryDbContext context, int count)
        {
            var customers = new List<PersonEntity>();
            var attributes = new List<AttributeValueEntity>();
            var random = new Random();

            var customerRole = await context.Roles.FirstAsync(r => r.RoleType == RoleType.Customer);
            var nameAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Name);
            var lastNameAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.LastName);
            var phoneNumberAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.PhoneNumber);
            var sexAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Sex);
            var emailAttribute = await context.Attributes.FirstAsync(a => a.Key == AttributeKey.Email);

            for (int i = 1; i <= count; i++)
            {
                var gender = (Gender)random.Next(0, 2);

                var customer = new PersonEntity
                {
                    Id = Guid.NewGuid(),
                    Status = PersonStatus.Active,
                    RegionId = random.Next(1, 80),
                    RoleId = customerRole.Id
                };

                customers.Add(customer);

                attributes.Add(AttributeValueHelper
                    .CreateAttribute(customer.Id, nameAttribute.Id, $"Customer {i}"));
                attributes.Add(AttributeValueHelper
                    .CreateAttribute(customer.Id, lastNameAttribute.Id, $"LastName {i}"));
                attributes.Add(AttributeValueHelper
                    .CreateAttribute(customer.Id, sexAttribute.Id, random.Next(0, 2).ToString()));
                attributes.Add(AttributeValueHelper
                    .CreateAttribute(customer.Id, phoneNumberAttribute.Id, $"{random.Next(100, 999)}-{random.Next(1000, 9999)}"));
                attributes.Add(AttributeValueHelper
                    .CreateAttribute(customer.Id, emailAttribute.Id, $"customer{i}@email.com"));
            }

            await TransactionHelper.ExecuteInTransactionAsync(context, async () =>
            {
                await context.Persons.AddRangeAsync(customers);
                await context.AttributeValues.AddRangeAsync(attributes);
                await context.SaveChangesAsync();
            });
        }
    }
}
