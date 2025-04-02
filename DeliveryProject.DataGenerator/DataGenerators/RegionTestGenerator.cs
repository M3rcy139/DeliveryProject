using System.Reflection;
using DeliveryProject.DataAccess;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataGenerator.Helpers;


namespace DeliveryProject.DataGenerator.DataGenerators
{
    public static class RegionTestGenerator
    {
        private const string ResourcePath = "DeliveryProject.DataGenerator.Resources.regions.json";
        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        public static async Task GenerateRegions(this DeliveryDbContext context)
        {
            var regions = Assembly.LoadFromResourceJson<RegionEntity>(ResourcePath);
            await context.Regions.AddRangeAsync(regions);
        }
    }
}
