using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.Tests.Helpers
{
    public static class OrderTestHelper
    {
        public static async Task GenerateOrders(this DeliveryDbContext context, int count)
        {
            var random = new Random();
            var orders = new List<OrderEntity>();

            for (int i = 1; i <= count; i++)
            {
                orders.Add(new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    Weight = Math.Round(random.NextDouble() * 10, 1),
                    RegionId = random.Next(1, 80),
                    DeliveryPersonId = i,
                    SupplierId = random.Next(1, 50),
                    DeliveryTime = new DateTime(2027, 10, 21, random.Next(8, 11), random.Next(0, 60), 0, DateTimeKind.Utc)
                });
            }

            await context.Orders.AddRangeAsync(orders);
        }
    }
}
