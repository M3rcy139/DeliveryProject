using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Tests.Helpers
{
    public static class CustomerTestHelper
    {
        public static async Task GenerateCustomers(this DeliveryDbContext context, int count)
        {
            var customers = new List<CustomerEntity>();
            var contacts = new List<PersonContactEntity>();
            var random = new Random();

            var customerRole = await context.Roles.FirstAsync(r => r.Role == RoleType.Customer);

            for (int i = 1; i <= count; i++)
            {
                var gender = (Gender)random.Next(0, 2);

                var customer = new CustomerEntity
                {
                    Id = Guid.NewGuid(),
                    Name = $"Customer {i}",
                    LastName = $"LastName {i}",
                    Gender = gender,
                    RoleId = customerRole.Id
                };

                customers.Add(customer);

                contacts.Add(new PersonContactEntity
                {
                    Id = Guid.NewGuid(),
                    PhoneNumber = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    Email = $"customer{i}@example.com",
                    RegionId = random.Next(1, 80), 
                    PersonId = customer.Id
                });
            }

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            await context.PersonContacts.AddRangeAsync(contacts);
            await context.SaveChangesAsync();
        }
    }
}
