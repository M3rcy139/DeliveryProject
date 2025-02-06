using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.Tets.Helpers
{
    internal class OrderTestHelper
    {
        private readonly DeliveryDbContext _context;
        private readonly int _orders;

        public OrderTestHelper(DeliveryDbContext context, int orders)
        {
            _context = context;
            _orders = orders;
        }

        public async Task GenerateAndSaveOrdersAsync()
        {
            var orders = new List<OrderEntity>();
            var random = new Random();

            for (int i = 1; i <= _orders; i++)
            {
                double weight = Math.Round(random.NextDouble() * 10, 1);
                int regionId = random.Next(1, 80);
                int deliveryPersonId = i;
                int supplierId = random.Next(1, 50);
                var deliveryTime = new DateTime(2027, 10, 21, random.Next(8, 11), random.Next(0, 60), 0, DateTimeKind.Utc);

                orders.Add(new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    Weight = weight,
                    RegionId = regionId,
                    DeliveryPersonId = deliveryPersonId,
                    SupplierId = supplierId,
                    DeliveryTime = deliveryTime
                });
            }

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{_orders} delivery persons generated and saved.");
        }
    }
}
