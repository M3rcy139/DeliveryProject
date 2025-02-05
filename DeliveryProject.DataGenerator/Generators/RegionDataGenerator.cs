using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess;

namespace DeliveryProject.DataGenerator.Generators
{
    internal class RegionDataGenerator
    {
        private readonly DeliveryDbContext _context;

        public RegionDataGenerator(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task GenerateAndSaveRegionsAsync()
        {
            var regions = new List<RegionEntity>();
            for (int i = 1; i <= 80; i++)
            {
                regions.Add(new RegionEntity
                {
                    Id = i,
                    Name = $"District {i}"
                });
            }

            await _context.Regions.AddRangeAsync(regions);
            await _context.SaveChangesAsync();
            Console.WriteLine("Regions generated and saved.");
        }
    }
}
