using DeliveryProject.DataAccess;
using DeliveryProject.Tests.DataGeneration;
using DeliveryProject.Tets.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DataGenerationEntryPoint
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting data generation...");

        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DeliveryDbContext");

            var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            await using var context = new DeliveryDbContext(optionsBuilder.Options);

            var dataSettings = configuration.GetSection("DataGenerationSettings").Get<DataGenerationSettings>();

            var regionGenerator = new RegionTestHelper(context, dataSettings.RegionsCount);
            var deliveryPersonGenerator = new DeliveryPersonTestHelper(context, dataSettings.DeliveryPersonsCount);
            var supplierGenerator = new SupplierTestHelper(context, dataSettings.SuppliersCount);
            var orderGenerator = new OrderTestHelper(context, dataSettings.OrdersCount);


            await regionGenerator.GenerateAndSaveRegionsAsync();
            await deliveryPersonGenerator.GenerateAndSaveDeliveryPersonsAsync();
            await supplierGenerator.GenerateAndSaveSuppliersAsync();
            await orderGenerator.GenerateAndSaveOrdersAsync();

            Console.WriteLine("Data generation completed and applied to the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}