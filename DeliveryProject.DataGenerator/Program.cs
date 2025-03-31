using DeliveryProject.DataAccess;
using DeliveryProject.DataGenerator.Configurations;
using DeliveryProject.DataGenerator.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeliveryProject.DataGenerator;

public class Program
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

            await context.GenerateAllData(dataSettings);

            Console.WriteLine("Data generation completed and applied to the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}