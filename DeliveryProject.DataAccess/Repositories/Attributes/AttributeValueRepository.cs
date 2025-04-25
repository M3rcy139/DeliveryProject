using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryProject.DataAccess.Repositories.Attributes
{
    public class AttributeValueRepository : IAttributeValueRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        
        public AttributeValueRepository(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        public async Task SetAttributeValue(Guid personId, AttributeKey attributeKey, string? value)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();

            var newAttributeValue = new AttributeValueEntity
            {
                Id = Guid.NewGuid(),
                PersonId = personId,
                AttributeId = (int)attributeKey,
                Value = value
            };
            dbContext.AttributeValues.Add(newAttributeValue);

            await dbContext.SaveChangesAsync();
        }

        public async Task<string?> GetAttributeValue(Guid personId, AttributeKey attributeKey)
        {
            using var scope = _serviceScopeFactory.CreateScope();  
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();  

            var attribute = await dbContext.AttributeValues
                .FirstOrDefaultAsync(av => av.PersonId == personId && av.Attribute.Key == attributeKey);

            return attribute?.Value;
        }
    }
}
