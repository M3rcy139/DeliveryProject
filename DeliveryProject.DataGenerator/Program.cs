using DeliveryProject.DataAccess;
using DeliveryProject.DataGenerator.Generators;
using Microsoft.EntityFrameworkCore;

class Program
{
    private const string ConnectionString = "Host=localhost;User ID=postgres;Password=12345;Port=5432;Database=delivery;";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting data generation...");

        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();
            optionsBuilder.UseNpgsql(ConnectionString);

            await using var context = new DeliveryDbContext(optionsBuilder.Options);

            var regionGenerator = new RegionDataGenerator(context);
            var deliveryPersonGenerator = new DeliveryPersonDataGenerator(context);
            var supplierGenerator = new SupplierDataGenerator(context);
            var orderGenerator = new OrderDataGenerator(context);

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