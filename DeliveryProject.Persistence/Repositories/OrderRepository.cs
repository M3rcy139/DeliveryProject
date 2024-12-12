using AutoMapper;
using DeliveryProject.Core.Models;
using DeliveryProject.Access.Entities;
using DeliveryProject.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Access.Repositories
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

        public async Task AddOrder(Order order)
        {
            var orderEntity = new OrderEntity()
            {
                Id = order.Id,
                Weight = order.Weight,
                AreaId = order.AreaId,
                DeliveryTime = order.DeliveryTime,
            };

            _context.Orders.Add(orderEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<DateTime> GetFirstOrderTime(int areaId)
        {
            var hasOrders = await _context.Orders
                .Where(o => o.AreaId == areaId)
                .AnyAsync();

            if (!hasOrders)
            {
                throw new ArgumentException($"Заказов в данном районе({areaId}) не найдено");
            }

            var firstOrder = await _context.Orders
                .Where(o => o.AreaId == areaId)
                .MinAsync(o => o.DeliveryTime);

            return firstOrder;
        }

        public async Task<List<Order>> GetOrdersWithinTimeRange(int areaId, DateTime fromTime, DateTime toTime)
        {
            var filteredOrders = await _context.Orders
                .Where(o => o.AreaId == areaId && o.DeliveryTime >= fromTime && o.DeliveryTime <= toTime)
                .ToListAsync();

            if (filteredOrders == null || filteredOrders.Count == 0)
            {
                throw new ArgumentException($"Заказы не найдены для района {areaId} в диапазоне времени с {fromTime} по {toTime}");
            }

            var filteredOrderEntities = filteredOrders.Select(o => new FilteredOrderEntity
            {
                Id = Guid.NewGuid(), 
                OrderId = o.Id,      
                AreaId = o.AreaId,   
                DeliveryTime = o.DeliveryTime, 
                Order = o,           
                Area = o.Area        
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
                throw new ArgumentException("Заказы не найдены");
            }

            return _mapper.Map<List<Order>>(orders);
        }
    }
}
