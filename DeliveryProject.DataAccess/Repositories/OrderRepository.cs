using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;
        private readonly Dictionary<Guid, OrderEntity> _orderCache = new();

        public OrderRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task AddOrder(OrderEntity orderEntity)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            dbContext.AttachRange(orderEntity.Persons);
            dbContext.AttachRange(orderEntity.OrderProducts.Select(op => op.Product));

            await dbContext.Orders.AddAsync(orderEntity);
            await dbContext.SaveChangesAsync();

            _orderCache[orderEntity.Id] = orderEntity;
        }

        public async Task<OrderEntity?> GetOrderById(Guid id)
        {
            if (_orderCache.TryGetValue(id, out var cachedOrder))
            {
                return cachedOrder;
            }

            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var order = await dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Persons)
                    .ThenInclude(p => p.Contacts)
                .Include(o => o.Persons)
                    .ThenInclude(p => p.Role)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
                _orderCache[id] = order;
            }
            return order;
        }

        public async Task UpdateOrder(OrderEntity orderEntity)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var existingEntity = await dbContext.Orders
                .Include(o => o.Persons)
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderEntity.Id);

            if (existingEntity != null)
            {
                dbContext.Entry(existingEntity).CurrentValues.SetValues(orderEntity);
            }

            existingEntity.Persons.Clear();
            foreach (var person in orderEntity.Persons)
            {
                var trackedPerson = await dbContext.Persons.FindAsync(person.Id);
                existingEntity.Persons.Add(trackedPerson ?? person);
            }

            existingEntity.OrderProducts.Clear();
            foreach (var orderProduct in orderEntity.OrderProducts)
            {
                var trackedProduct = await dbContext.Products.FindAsync(orderProduct.ProductId);
                if (trackedProduct != null)
                {
                    existingEntity.OrderProducts.Add(new OrderProductEntity
                    {
                        OrderId = orderEntity.Id,
                        ProductId = trackedProduct.Id,
                        Quantity = orderProduct.Quantity
                    });
                }
            }

            await dbContext.SaveChangesAsync();

            _orderCache[orderEntity.Id] = orderEntity;
        }

        public async Task DeleteOrder(Guid id)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var order = await dbContext.Orders.FindAsync(id);
            if (order != null)
            {
                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync();

                _orderCache.Remove(id, out _);
            }
        }

        public async Task<RegionEntity> GetRegionByName(string regionName)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var region = await dbContext.Regions
                .AsNoTracking() 
                .Where(r => r.Name.ToLower() == regionName.ToLower())
                .FirstOrDefaultAsync();

            return region;
        }
        public async Task<bool> HasOrders(int regionId)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Orders.AnyAsync(o => o.Persons.Any(p => p.Contacts.Any(c => c.RegionId == regionId)));
        }

        public async Task<DateTime> GetFirstOrderTime(int regionId)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            return await dbContext.Orders
                .Where(o => o.Persons.Any(p => p.Contacts.Any(c => c.RegionId == regionId)))
                .Select(o => o.DeliveryTime)
                .OrderBy(dt => dt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderEntity>> GetOrdersWithinTimeRange(int regionId, DateTime fromTime, DateTime toTime)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var filteredOrders = await dbContext.Orders
                .Where(o => o.Persons.Any(p => p.Contacts.Any(c => c.RegionId == regionId)) &&
                            o.DeliveryTime >= fromTime && o.DeliveryTime <= toTime)
                .ToListAsync();

            var filteredOrderEntities = filteredOrders.Select(o => new FilteredOrderEntity
            {
                Id = Guid.NewGuid(),
                OrderId = o.Id,
                Order = o
            }).ToList();

            await dbContext.FilteredOrders.AddRangeAsync(filteredOrderEntities);
            await dbContext.SaveChangesAsync();

            return filteredOrders;
        }

        public async Task<List<OrderEntity>> GetAllOrdersImmediate()
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var orders = await dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Persons)
                    .ThenInclude(p => p.Contacts)
                .Include(o => o.Persons) 
                    .ThenInclude(p => p.Role)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();

            return new List<OrderEntity>(orders);
        }
    }
}
