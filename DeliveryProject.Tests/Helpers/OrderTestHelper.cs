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
            //var random = new Random();
            //var orders = new List<OrderEntity>();

            //var customers = await context.Persons
            //    .Where(p => p.Role.Role == RoleType.Customer)
            //    .ToListAsync();

            //var deliveryPersons = await context.Persons
            //    .Where(p => p.Role.Role == RoleType.DeliveryPerson)
            //    .ToListAsync();

            //var suppliers = await context.Persons
            //    .Where(p => p.Role.Role == RoleType.Supplier)
            //    .ToListAsync();

            //var products = await context.Products.ToListAsync();

            //for (int i = 0; i < count; i++)
            //{
            //    var customer = customers[random.Next(customers.Count)];
            //    var deliveryPerson = deliveryPersons[random.Next(deliveryPersons.Count)];
            //    var supplier = suppliers[random.Next(suppliers.Count)];

            //    var orderProducts = new List<OrderProductEntity>();

            //    int productCount = random.Next(1, 5); 
            //    var selectedProducts = products.OrderBy(p => random.Next()).Take(productCount).ToList();

            //    foreach (var product in selectedProducts)
            //    {
            //        orderProducts.Add(new OrderProductEntity
            //        {
            //            ProductId = product.Id,
            //            Quantity = random.Next(1, 5) 
            //        });
            //    }

            //    var order = new OrderEntity
            //    {
            //        Id = Guid.NewGuid(),
            //        DeliveryTime = new DateTime(2027, 10, 21, random.Next(8, 11), random.Next(0, 60), 0, DateTimeKind.Utc),
            //        Persons = new List<PersonEntity> { customer, deliveryPerson, supplier },
            //        OrderProducts = orderProducts,
            //        Amount = orderProducts.Sum(op => op.Quantity * products.First(p => p.Id == op.ProductId).Price) 
            //    };

            //    orders.Add(order);
            //}

            //await context.Orders.AddRangeAsync(orders);
        }
    }
}
