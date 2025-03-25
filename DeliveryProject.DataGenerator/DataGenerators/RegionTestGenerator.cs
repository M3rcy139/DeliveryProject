using System.Reflection;
using System.Text.Json;
using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataGenerator.DataGenerators
{
    public static class RegionTestGenerator
    {
        public static async Task GenerateRegions(this DeliveryDbContext context, int count)
        {
            var regions = LoadRegionsFromJson();
            await context.Regions.AddRangeAsync(regions);
        }
        
        private static List<RegionEntity> LoadRegionsFromJson()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DeliveryProject.DataGenerator.Resources.regions.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream!))
            {
                var json = reader.ReadToEnd();
                return JsonSerializer.Deserialize<List<RegionEntity>>(json)!;
            }
        }
    }
}
