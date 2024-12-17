using AutoMapper;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DeliveryDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(DeliveryDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddOrder(Order order)
        {
            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                Weight = order.Weight,
                RegionId = order.RegionId,
                DeliveryTime = order.DeliveryTime,
            };

            await _context.Orders.AddAsync(orderEntity);
            
            return await _context.SaveChangesAsync();
        }

        public async Task<Region> GetRegionByName(string regionName)
        {
            var region = await _context.Regions
                .AsNoTracking() 
                .Include(r => r.Orders)
                .Where(r => r.Name.ToLower() == regionName.ToLower())
                .FirstOrDefaultAsync();

            if (region == null)
            {
                throw new BussinessArgumentException($"Регион с названием {regionName} не найден.");
            }

            return _mapper.Map<Region>(region);
        }

        public async Task<DateTime> GetFirstOrderTime(int regionId)
        {
            var hasOrders = await _context.Orders
                .Where(o => o.RegionId == regionId)
                .AnyAsync();

            if (!hasOrders)
            {
                throw new BussinessArgumentException($"Заказов в данном районе({regionId}) не найдено");
            }

            var firstOrder = await _context.Orders
                .Where(o => o.RegionId == regionId)
                .MinAsync(o => o.DeliveryTime);

            return firstOrder;
        }

        public async Task<List<Order>> GetOrdersWithinTimeRange(int regionId, DateTime fromTime, DateTime toTime)
        {
            var filteredOrders = await _context.Orders
                .Where(o => o.RegionId == regionId && o.DeliveryTime >= fromTime && o.DeliveryTime <= toTime)
                .ToListAsync();

            if (filteredOrders == null || filteredOrders.Count == 0)
            {
                throw new BussinessArgumentException($"Заказы не найдены для района {regionId} в диапазоне времени с {fromTime} по {toTime}");
            }

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

            return _mapper.Map<List<Order>>(filteredOrders);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                throw new BussinessArgumentException("Заказы не найдены");
            }

            return _mapper.Map<List<Order>>(orders);
        }
    }
}
