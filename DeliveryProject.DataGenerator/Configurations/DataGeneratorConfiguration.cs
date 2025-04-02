using DeliveryProject.DataAccess;
using DeliveryProject.DataGenerator.DataGenerators;
using DeliveryProject.DataGenerator.Settings;

namespace DeliveryProject.DataGenerator.Configurations;

public static class DataGeneratorConfiguration
{
    public static async Task GenerateAllData(this DeliveryDbContext context, DataGenerationSettings settings)
    {
        await context.GenerateRegions();
        await context.SaveChangesAsync();

        await context.GenerateDeliveryPersons(settings.DeliveryPersonsCount);
        await context.GenerateSuppliers(settings.SuppliersCount);
        await context.GenerateCustomers(settings.CustomersCount);
        await context.GenerateOrders(settings.OrdersCount);

        await context.SaveChangesAsync();
    }
}