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
            using var dbContext = _contextFactory.CreateDbContext();
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

            using var dbContext = _contextFactory.CreateDbContext();
            var order = await dbContext.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                _orderCache[id] = order;
            }
            return order;
        }

        public async Task UpdateOrder(OrderEntity orderEntity)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            var existingEntity = await dbContext.Orders.FindAsync(orderEntity.Id);

            if (existingEntity != null)
            {
                dbContext.Entry(existingEntity).CurrentValues.SetValues(orderEntity);
            }

            await dbContext.SaveChangesAsync();

            _orderCache[orderEntity.Id] = orderEntity;
        }

        public async Task DeleteOrder(Guid id)
        {
            using var dbContext = _contextFactory.CreateDbContext();
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
            using var dbContext = _contextFactory.CreateDbContext();
            var region = await dbContext.Regions
                .AsNoTracking() 
                .Include(r => r.Orders)
                .Where(r => r.Name.ToLower() == regionName.ToLower())
                .FirstOrDefaultAsync();

            return region;
        }
        public async Task<bool> HasOrders(int regionId)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.Orders.AnyAsync(o => o.RegionId == regionId);
        }  

        public async Task<DateTime> GetFirstOrderTime(int regionId)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.Orders.Where(o => o.RegionId == regionId).MinAsync(o => o.DeliveryTime);
        } 

        public async Task<List<OrderEntity>> GetOrdersWithinTimeRange(int regionId, DateTime fromTime, DateTime toTime)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            var filteredOrders = await dbContext.Orders
                .Where(o => o.RegionId == regionId && o.DeliveryTime >= fromTime && o.DeliveryTime <= toTime)
                .ToListAsync();

            var filteredOrderEntities = filteredOrders.Select(o => new FilteredOrderEntity
            {
                Id = Guid.NewGuid(), 
                OrderId = o.Id,      
                RegionId = o.RegionId,   
                DeliveryTime = o.DeliveryTime, 
                Order = o,           
                Region = o.Region,    
                DeliveryPersonId = o.DeliveryPersonId,
                SupplierId = o.SupplierId,
            }).ToList();

            await dbContext.FilteredOrders.AddRangeAsync(filteredOrderEntities);
            await dbContext.SaveChangesAsync();

            return filteredOrders;
        }

        public async Task<List<OrderEntity>> GetAllOrdersImmediate()
        {
            using var dbContext = _contextFactory.CreateDbContext();
            var orders = await dbContext.Orders.AsNoTracking().ToListAsync();

            return new List<OrderEntity>(orders);
        }
    }
}
