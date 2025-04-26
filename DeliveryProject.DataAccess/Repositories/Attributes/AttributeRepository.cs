using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryProject.DataAccess.Repositories.Attributes
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly DeliveryDbContext _dbContext;

        public AttributeRepository(DeliveryDbContext dbContext) => _dbContext = dbContext;

        public async Task<Dictionary<AttributeKey, int>> GetAttributeIdsByKeys(IEnumerable<AttributeKey> keys)
        {
            return await _dbContext.Attributes
                .Where(a => keys.Contains(a.Key))
                .ToDictionaryAsync(a => a.Key, a => a.Id);
        }
    }
}
