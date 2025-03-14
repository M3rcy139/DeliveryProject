using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories.Common
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly IDbContextFactory<DeliveryDbContext> _contextFactory;

        public AttributeRepository(IDbContextFactory<DeliveryDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<Dictionary<AttributeKey, int>> GetAttributeIdsByKeys(IEnumerable<AttributeKey> keys)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();

            return await dbContext.Attributes
                .Where(a => keys.Contains(a.Key))
                .ToDictionaryAsync(a => a.Key, a => a.Id);
        }
    }
}
