using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;
        private readonly Dictionary<Guid, OrderEntity> _orderCache = new();

        public OrderRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task AddOrder(OrderEntity orderEntity)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            foreach (var orderPerson in orderEntity.OrderPersons)
            {
                var person = orderPerson.Person;

                dbContext.Entry(person).State = EntityState.Unchanged;
            }

            foreach (var orderProduct in orderEntity.OrderProducts)
            {
                var product = orderProduct.Product;

                dbContext.Entry(product).State = EntityState.Unchanged;
            }

            await dbContext.Orders.AddAsync(orderEntity);
            await dbContext.SaveChangesAsync();

            _orderCache[orderEntity.Id] = orderEntity;
        }

        public async Task<OrderEntity> GetOrderById(Guid id)
        {
            if (_orderCache.TryGetValue(id, out var cachedOrder))
            {
                return cachedOrder;
            }

            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var order = await dbContext.Orders
                .AsNoTracking()
                .Include(o => o.OrderPersons)
                    .ThenInclude(op => op.Person)
                        .ThenInclude(p => p.Role)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Invoice)
                .FirstAsync(o => o.Id == id);

            _orderCache[id] = order;

            return order;
        }

        public async Task UpdateOrder(OrderEntity orderEntity)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();

            var existingEntity = await dbContext.Orders
                .Include(o => o.OrderPersons)
                    .ThenInclude(op => op.Person)
                .Include(o => o.OrderProducts)
                .Include(o => o.Invoice)
                .FirstAsync(o => o.Id == orderEntity.Id);

            dbContext.Entry(existingEntity).CurrentValues.SetValues(orderEntity);

            var orderUpdater = new OrderUpdater(dbContext);
            await orderUpdater.UpdateOrder(existingEntity, orderEntity);

            await dbContext.SaveChangesAsync();
            _orderCache[orderEntity.Id] = orderEntity;
        }

        public async Task DeleteOrder(Guid id)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var order = await dbContext.Orders.FindAsync(id);
            
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync();

            _orderCache.Remove(id, out _);
        }

        public async Task<List<OrderEntity>> GetAllOrdersImmediate()
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var orders = await dbContext.Orders
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
