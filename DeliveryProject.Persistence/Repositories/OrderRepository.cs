using AutoMapper;
using DeliveryProject.Core.Models;
using DeliveryProject.Persistence.Entities;
using DeliveryProject.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Persistence.Repositories
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
                Id = Guid.NewGuid(),
                Weight = order.Weight,
                AreaId = order.AreaId,
                DeliveryTime = order.DeliveryTime,
            };

            _context.Orders.Add(orderEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> FilterOrder(int areaId, DateTime fromTime, DateTime toTime)
        {
            var filteredOrders = await _context.Orders
                .Where(o => o.AreaId == areaId && o.DeliveryTime >= fromTime && o.DeliveryTime <= toTime)
                .ToListAsync();

            return _mapper.Map<List<Order>>(filteredOrders);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            return _mapper.Map<List<Order>>(orders);
        }
    }
}
