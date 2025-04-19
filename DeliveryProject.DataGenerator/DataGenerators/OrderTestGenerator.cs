using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataGenerator.Helpers;

namespace DeliveryProject.DataGenerator.DataGenerators
{
    public static class OrderTestGenerator
    {
        public static async Task GenerateOrders(this DeliveryDbContext context, int count)
        {
            var random = new Random();
            var orders = new List<OrderEntity>();
            var invoices = new List<InvoiceEntity>();

            var customers = await context.Persons
                .OfType<CustomerEntity>()
                .ToListAsync();

            var deliveryPersons = await context.Persons
                .OfType<DeliveryPersonEntity>()
                .ToListAsync();

            var products = await context.Products.ToListAsync();

            for (int i = 0; i < count; i++)
            {
                var customer = customers[random.Next(customers.Count)];
                var deliveryPerson = deliveryPersons[random.Next(deliveryPersons.Count)];

                var orderProducts = OrderProductGenerator.Generate(products, random);
                var amount = orderProducts.Sum(op => op.Quantity * products.First(p => p.Id == op.ProductId).Price);

                var order = new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTime.UtcNow,
                    Status = OrderStatus.Active,
                    Amount = amount,
                    OrderPersons = new List<OrderPersonEntity>
                {
                    new OrderPersonEntity { PersonId = customer.Id },
                },
                    OrderProducts = orderProducts,
                };

                orders.Add(order);

                var invoice = InvoiceTestHelper.CreateInvoice(order.Id, deliveryPerson.Id);
                invoices.Add(invoice);
            }

            await context.Orders.AddRangeAsync(orders);
            await context.Invoices.AddRangeAsync(invoices);
            await context.SaveChangesAsync();
        }
    }
}

