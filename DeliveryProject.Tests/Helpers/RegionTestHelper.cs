using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.Tets.Helpers
{
    internal class RegionTestHelper
    {
        private readonly DeliveryDbContext _context;
        private readonly int _regionsCount;

        public RegionTestHelper(DeliveryDbContext context, int regionsCount)
        {
            _context = context;
            _regionsCount = regionsCount;
        }

        public async Task GenerateAndSaveRegionsAsync()
        {
            var regions = new List<RegionEntity>();
            for (int i = 1; i <= _regionsCount; i++)
            {
                regions.Add(new RegionEntity
                {
                    Id = i,
                    Name = $"District {i}"
                });
            }

            await _context.Regions.AddRangeAsync(regions);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{_regionsCount} regions generated and saved.");
        }
    }
}
