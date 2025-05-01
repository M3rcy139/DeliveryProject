using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DeliveryDbContext _dbContext;
        private readonly Dictionary<Guid, OrderEntity> _orderCache = new();
        private readonly OrderUpdater _orderUpdater;

        public OrderRepository(DeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
            _orderUpdater = new OrderUpdater(_dbContext);
        }

        public async Task AddOrder(OrderEntity orderEntity)
        {
            _dbContext.AttachRange(orderEntity.OrderPersons.Select(op => op.Person));
            _dbContext.AttachRange(orderEntity.OrderProducts.Select(op => op.Product));

            await _dbContext.Orders.AddAsync(orderEntity);

            _orderCache[orderEntity.Id] = orderEntity;
        }

        public async Task<OrderEntity?> GetOrderById(Guid id)
        {
            if (_orderCache.TryGetValue(id, out var cachedOrder))
            {
                return cachedOrder;
            }
            
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.OrderPersons)
                    .ThenInclude(op => op.Person)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if(order != null)
                _orderCache[id] = order;

            return order;
        }

        public async Task UpdateOrderProducts(OrderEntity orderEntity)
        {
            var existingEntity = await _dbContext.Orders
                .Include(o => o.OrderPersons)
                    .ThenInclude(op => op.Person)
                .Include(o => o.OrderProducts)
                .FirstAsync(o => o.Id == orderEntity.Id);

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(orderEntity);
            
            await _orderUpdater.UpdateOrder(existingEntity, orderEntity);

            _orderCache[orderEntity.Id] = orderEntity;
        }

        public void UpdateOrderStatus(OrderEntity orderEntity)
        {
            _dbContext.Orders.Attach(orderEntity); 
            _dbContext.Entry(orderEntity).Property(o => o.Status).IsModified = true;
        }
        
        public async Task DeleteOrder(Guid id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            
            _dbContext.Orders.Remove(order!);

            _orderCache.Remove(id, out _);
        }

        public async Task<List<OrderEntity>> GetAllOrders()
        {
            var orders = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.OrderPersons)
                    .ThenInclude(op => op.Person)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();

            return new List<OrderEntity>(orders);
        }
    }
}
