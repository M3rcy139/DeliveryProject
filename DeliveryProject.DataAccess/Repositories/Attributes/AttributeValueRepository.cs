using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Attributes
{
    public class AttributeValueRepository : IAttributeValueRepository
    {
        private readonly DeliveryDbContext _dbContext;

        public AttributeValueRepository(DeliveryDbContext dbContext) => _dbContext = dbContext;

        public async Task SetAttributeValue(Guid personId, AttributeKey attributeKey, string? value)
        {
            var newAttributeValue = new AttributeValueEntity
            {
                Id = Guid.NewGuid(),
                PersonId = personId,
                AttributeId = (int)attributeKey, 
                Value = value
            };
            _dbContext.AttributeValues.Add(newAttributeValue);

            _dbContext.SaveChanges();
        }

        public async Task<string?> GetAttributeValue(Guid personId, AttributeKey attributeKey)
        {
            var attribute = _dbContext.AttributeValues
                .FirstOrDefault(av => av.PersonId == personId && av.Attribute.Key == attributeKey);

            return attribute?.Value;
        }
    }
}