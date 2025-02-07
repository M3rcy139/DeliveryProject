using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Tests.Helpers
{
    public static class RegionTestHelper
    {
        public static async Task GenerateRegions(this DeliveryDbContext context, int count)
        {
            var regions = Enumerable.Range(1, count)
                .Select(i => new RegionEntity { Id = i, Name = $"Region {i}" })
                .ToList();

            await context.Regions.AddRangeAsync(regions);
        }
    }
}
