using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Extensions;
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
                .ThenInclude(op => op.Person)
                .Include(o => o.OrderProducts)
                .Include(o => o.Invoice)
                .FirstOrDefaultAsync(o => o.Id == orderEntity.Id);

            if (existingEntity != null)
                dbContext.Entry(existingEntity).CurrentValues.SetValues(orderEntity);

            if (existingEntity.Invoice != null && orderEntity.Invoice != null)
            {
                existingEntity.Invoice.Amount = orderEntity.Invoice.Amount;
            }

            await UpdateOrderPersons(dbContext, existingEntity, orderEntity);
            UpdateOrderProducts(dbContext, existingEntity, orderEntity);

            await dbContext.SaveChangesAsync();
            _orderCache[orderEntity.Id] = orderEntity;
        }

        private async Task UpdateOrderPersons(DeliveryDbContext dbContext, OrderEntity existingEntity, OrderEntity orderEntity)
        {
            existingEntity.OrderPersons.Clear();

            foreach (var newOrderPerson in orderEntity.OrderPersons)
            {
                var attachedPerson = await dbContext.Persons.FindAsync(newOrderPerson.PersonId);
                if (attachedPerson == null)
                {
                    attachedPerson = new PersonEntity { Id = newOrderPerson.PersonId };
                    dbContext.Attach(attachedPerson);
                }

                existingEntity.OrderPersons.Add(new OrderPersonEntity
                {
                    OrderId = orderEntity.Id,
                    PersonId = attachedPerson.Id,
                    Person = attachedPerson
                });
            }
        }

        private void UpdateOrderProducts(DeliveryDbContext dbContext, OrderEntity existingEntity, OrderEntity orderEntity)
        {
            var newProducts = orderEntity.OrderProducts.ToList();
            var existingProducts = existingEntity.OrderProducts.ToList();

            foreach (var existingProduct in existingProducts)
            {
                if (!newProducts.Any(np => np.ProductId == existingProduct.ProductId))
                {
                    dbContext.Remove(existingProduct);
                }
            }

            foreach (var newProduct in newProducts)
            {
                var existingProduct = existingProducts.FirstOrDefault(ep => ep.ProductId == newProduct.ProductId);
                if (existingProduct == null)
                {
                    existingEntity.OrderProducts.Add(newProduct);
                }
                else
                {
                    existingProduct.Quantity = newProduct.Quantity;
                }
            }
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
