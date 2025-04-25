using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryProject.DataAccess.Repositories.Attributes
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AttributeRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Dictionary<AttributeKey, int>> GetAttributeIdsByKeys(IEnumerable<AttributeKey> keys)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
                return await dbContext.Attributes
                    .Where(a => keys.Contains(a.Key))
                    .ToDictionaryAsync(a => a.Key, a => a.Id);
            }
        }
    }
}
