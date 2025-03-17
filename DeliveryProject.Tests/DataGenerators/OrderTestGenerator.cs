using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.Tests.DataGenerators
{
    public static class OrderTestGenerator
    {
        public static async Task GenerateOrders(this DeliveryDbContext context, int count)
        {
            var random = new Random();
            var orders = new List<OrderEntity>();

            var customers = await context.Persons
                .Where(p => p.Role.RoleType == RoleType.Customer)
                .ToListAsync();

            var deliveryPersons = await context.Persons
                .Where(p => p.Role.RoleType == RoleType.DeliveryPerson)
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
                    OrderPersons = new List<OrderPersonEntity>
                {
                    new OrderPersonEntity { PersonId = customer.Id },
                    new OrderPersonEntity { PersonId = deliveryPerson.Id },
                },
                    OrderProducts = orderProducts,
                    Invoice = new InvoiceEntity
                    {
                        Id = Guid.NewGuid(),
                        OrderId = Guid.NewGuid(),
                        DeliveryPersonId = deliveryPerson.Id,
                        Amount = amount,
                        DeliveryTime = new DateTime(2027, 10, 21, random.Next(8, 11), random.Next(0, 60), 0, DateTimeKind.Utc),
                        IsExecuted = false
                    }
                };

                orders.Add(order);
            }

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }
    }
}

