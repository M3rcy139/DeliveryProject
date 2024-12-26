using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DeliveryDbContext _context;

        public OrderRepository(DeliveryDbContext context) 
        {
            _context = context;
        }

        public async Task<int> AddOrder(OrderEntity orderEntity)
        {
            await _context.Orders.AddAsync(orderEntity);
            
            return await _context.SaveChangesAsync();
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
        public async Task<bool> HasOrders(int regionId)
        {
            return await _context.Orders
                .Where(o => o.RegionId == regionId)
                .AnyAsync();
        }

        public async Task<DateTime> GetFirstOrderTime(int regionId)
        {
            return  await _context.Orders
                .Where(o => o.RegionId == regionId)
                .MinAsync(o => o.DeliveryTime);
        }

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
                Region = o.Region        
            }).ToList();

            await _context.FilteredOrders.AddRangeAsync(filteredOrderEntities);
            await _context.SaveChangesAsync();

            return filteredOrders;
        }

        public async Task<List<OrderEntity>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            return orders;
        }
    }
}
