using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Repositories.Orders
{
    public class OrderUpdater
    {
        private readonly DeliveryDbContext _dbContext;

        public OrderUpdater(DeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateOrder(OrderEntity existingEntity, OrderEntity newEntity)
        {
            UpdateInvoice(existingEntity, newEntity);
            await UpdateOrderRelationships(existingEntity, newEntity);
        }

        private void UpdateInvoice(OrderEntity existingEntity, OrderEntity newEntity)
        {
            if (existingEntity.Invoice != null && newEntity.Invoice != null)
            {
                existingEntity.Invoice.Amount = newEntity.Invoice.Amount;
            }
        }

        private async Task UpdateOrderRelationships(OrderEntity existingEntity, OrderEntity newEntity)
        {
            await UpdateOrderPersons(existingEntity, newEntity);
            UpdateOrderProducts(existingEntity, newEntity);
        }

        private async Task UpdateOrderPersons(OrderEntity existingEntity, OrderEntity newEntity)
        {
            existingEntity.OrderPersons.Clear();
            await ClearAndAttachNewPersons(existingEntity, newEntity);
        }

        private async Task ClearAndAttachNewPersons(OrderEntity existingEntity, OrderEntity newEntity)
        {
            foreach (var newOrderPerson in newEntity.OrderPersons)
            {
                var attachedPerson = await AttachPerson(newOrderPerson.PersonId);
                existingEntity.OrderPersons.Add(new OrderPersonEntity
                {
                    OrderId = newEntity.Id,
                    PersonId = attachedPerson.Id,
                    Person = attachedPerson
                });
            }
        }

        private async Task<PersonEntity> AttachPerson(Guid personId)
        {
            var person = await _dbContext.Persons.FindAsync(personId);
            _dbContext.Attach(person);
            return person;
        }

        private void UpdateOrderProducts(OrderEntity existingEntity, OrderEntity newEntity)
        {
            RemoveOldProducts(existingEntity, newEntity);
            AddOrUpdateProducts(existingEntity, newEntity);
        }

        private void RemoveOldProducts(OrderEntity existingEntity, OrderEntity newEntity)
        {
            var existingProducts = existingEntity.OrderProducts.ToList();
            foreach (var existingProduct in existingProducts)
            {
                if (!newEntity.OrderProducts.Any(np => np.ProductId == existingProduct.ProductId))
                {
                    _dbContext.Remove(existingProduct);
                }
            }
        }

        private void AddOrUpdateProducts(OrderEntity existingEntity, OrderEntity newEntity)
        {
            foreach (var newProduct in newEntity.OrderProducts)
            {
                var existingProduct = existingEntity.OrderProducts.FirstOrDefault(ep => ep.ProductId == newProduct.ProductId);
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
    }

}
