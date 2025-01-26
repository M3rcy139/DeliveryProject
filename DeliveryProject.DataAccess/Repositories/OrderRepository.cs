using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace DeliveryProject.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DeliveryDbContext _context;
        private readonly Dictionary<Guid, OrderEntity> _orderCache = new();
        private readonly SemaphoreSlim _dbContextLock = new SemaphoreSlim(1, 1);

        public OrderRepository(DeliveryDbContext context) => _context = context;

        public async Task AddOrder(OrderEntity orderEntity)
        {
            await _dbContextLock.WaitAsync();
            try
            {
                await _context.Orders.AddAsync(orderEntity);
                await _context.SaveChangesAsync();

                _orderCache[orderEntity.Id] = orderEntity;
            }
            finally
            {
                _dbContextLock.Release();
            }
        }

        public async Task<OrderEntity?> GetOrderById(Guid id)
        {
            await _dbContextLock.WaitAsync();
            try
            {
                if (_orderCache.TryGetValue(id, out var cachedOrder))
                {
                    return cachedOrder;
                }

                var order = await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    _orderCache[id] = order; 
                }
                return order;
            }
            finally
            {
                _dbContextLock.Release();
            }
        }

        public async Task UpdateOrder(OrderEntity orderEntity)
        {
            await _dbContextLock.WaitAsync();
            try
            {
                var existingEntity = await _context.Orders.FindAsync(orderEntity.Id);

                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(orderEntity);
                }

                await _context.SaveChangesAsync();
            }
            finally
            {
                _dbContextLock.Release();
            }

            _orderCache[orderEntity.Id] = orderEntity;
        }

        public async Task DeleteOrder(Guid id)
        {
            await _dbContextLock.WaitAsync();
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();

                    _orderCache.Remove(id, out _); 
                }
            }
            finally
            {
                _dbContextLock.Release();
            }
        }

        public async Task<RegionEntity> GetRegionByName(string regionName)
        {
            var region = await _context.Regions
                .AsNoTracking() 
                .Include(r => r.Orders)
                .Where(r => r.Name.ToLower() == regionName.ToLower())
                .FirstOrDefaultAsync();

            return region;
        }
        public async Task<bool> HasOrders(int regionId) =>
            await _context.Orders.AnyAsync(o => o.RegionId == regionId);

        public async Task<DateTime> GetFirstOrderTime(int regionId) =>
            await _context.Orders.Where(o => o.RegionId == regionId).MinAsync(o => o.DeliveryTime);

        public async Task<List<OrderEntity>> GetOrdersWithinTimeRange(int regionId, DateTime fromTime, DateTime toTime)
        {
            var filteredOrders = await _context.Orders
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

            await _context.FilteredOrders.AddRangeAsync(filteredOrderEntities);
            await _context.SaveChangesAsync();

            return filteredOrders;
        }

        public async Task<ConcurrentBag<OrderEntity>> GetAllOrdersImmediate()
        {
            var orders = await _context.Orders.AsNoTracking().ToListAsync();
            return new ConcurrentBag<OrderEntity>(orders);
        }
    }
}
