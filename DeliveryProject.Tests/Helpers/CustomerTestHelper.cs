using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Tests.Helpers
{
    public static class CustomerTestHelper
    {
        public static async Task GenerateCustomers(this DeliveryDbContext context, int count)
        {
            //var customers = new List<CustomerEntity>();
            //var contacts = new List<PersonContactEntity>();
            //var random = new Random();

            //var customerRole = await context.Roles.FirstAsync(r => r.Role == RoleType.Customer);

            //for (int i = 1; i <= count; i++)
            //{
            //    var gender = (Gender)random.Next(0, 2);

            //    var customer = new CustomerEntity
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = $"Customer {i}",
            //        LastName = $"LastName {i}",
            //        Gender = gender,
            //        RoleId = customerRole.Id
            //    };

            //    customers.Add(customer);

            //    var contact = PersonContactHelper.CreatePersonContact(customer.Id, random, i, "customer");
            //    contacts.Add(contact);
            //}

            //await TransactionHelper.ExecuteInTransactionAsync(context, async () =>
            //{
            //    await context.Customers.AddRangeAsync(customers);
            //    await context.PersonContacts.AddRangeAsync(contacts);
            //    await context.SaveChangesAsync();
            //});
        }
    }
}
