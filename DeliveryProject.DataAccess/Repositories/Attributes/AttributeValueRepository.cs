using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Attributes
{
    public class AttributeValueRepository : IAttributeValueRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public AttributeValueRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task SetAttributeValue(Guid personId, AttributeKey attributeKey, string? value)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();

            var newAttributeValue = new AttributeValueEntity
            {
                Id = Guid.NewGuid(),
                PersonId = personId,
                AttributeId = (int)attributeKey, 
                Value = value
            };
            dbContext.AttributeValues.Add(newAttributeValue);

            dbContext.SaveChanges();
        }

        public async Task<string?> GetAttributeValue(Guid personId, AttributeKey attributeKey)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();

            var attribute = dbContext.AttributeValues
                .FirstOrDefault(av => av.PersonId == personId && av.Attribute.Key == attributeKey);

            return attribute?.Value;
        }
    }
}
