using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Tests.Helpers;

namespace DeliveryProject.Tests.DataGenerators
{
    public static class CustomerTestGenerator
    {
        public static async Task GenerateCustomers(this DeliveryDbContext context, int count)
        {
            var customers = new List<PersonEntity>();
            var attributes = new List<AttributeValueEntity>();
            var random = new Random();

            var customerRole = await context.Roles.FirstAsync(r => r.RoleType == RoleType.Customer);

            for (int i = 1; i <= count; i++)
            {
                var sex = (Sex)random.Next(0, 2);

                var customer = new CustomerEntity
                {
                    Status = PersonStatus.Active,
                    RegionId = random.Next(1, 80),
                    RoleId = customerRole.Id,
                    Name = $"Customer {i}",
                    LastName = $"Customer LastName{i}",
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    Sex = sex,
                    Email = $"customer{i}@email.com",
                };

                customers.Add(customer);
            }

            await TransactionTestHelper.ExecuteInTransactionAsync(context, async () =>
            {
                await context.Persons.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            });
        }
    }
}
