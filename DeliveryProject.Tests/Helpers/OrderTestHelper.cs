using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.Tests.Helpers
{
    public static class OrderTestHelper
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

                var orderProducts = GenerateOrderProducts(products, random);
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

        private static List<OrderProductEntity> GenerateOrderProducts(List<ProductEntity> products, Random random)
        {
            var orderProducts = new List<OrderProductEntity>();

            int productCount = random.Next(1, 5);
            var selectedProducts = products.OrderBy(_ => random.Next()).Take(productCount).ToList();

            foreach (var product in selectedProducts)
            {
                orderProducts.Add(new OrderProductEntity
                {
                    ProductId = product.Id,
                    Quantity = random.Next(1, 5)
                });
            }

            return orderProducts;
        }
    }
}

