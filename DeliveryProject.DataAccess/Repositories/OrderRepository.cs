using DeliveryProject.Core.Enums;
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
            dbContext.AttachRange(orderEntity.OrderPersons.Select(op => op.Person));
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
                .Include(o => o.OrderPersons)
                    .ThenInclude(op => op.Person)
                        .ThenInclude(p => p.Role)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Invoice)
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
                .Include(o => o.OrderPersons)
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderEntity.Id);

            if (existingEntity != null)
            {
                dbContext.Entry(existingEntity).CurrentValues.SetValues(orderEntity);
            }

            var existingOrderPersons = existingEntity.OrderPersons.ToList();
            existingEntity.OrderPersons.Clear();

            foreach (var newOrderPerson in orderEntity.OrderPersons)
            {
                if (!existingOrderPersons.Any(eop => eop.PersonId == newOrderPerson.PersonId))
                {
                    existingEntity.OrderPersons.Add(new OrderPersonEntity
                    {
                        OrderId = orderEntity.Id,
                        PersonId = newOrderPerson.PersonId
                    });
                }
            }

            var existingOrderProducts = existingEntity.OrderProducts.ToList();
            existingEntity.OrderProducts.Clear();

            foreach (var newOrderProduct in orderEntity.OrderProducts)
            {
                if (!existingOrderProducts.Any(eop => eop.ProductId == newOrderProduct.ProductId))
                {
                    existingEntity.OrderProducts.Add(new OrderProductEntity
                    {
                        OrderId = orderEntity.Id,
                        ProductId = newOrderProduct.ProductId,
                        Quantity = newOrderProduct.Quantity
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
