using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataGenerator.Helpers;

public static class ProductTestHelper
{
    private static readonly Random Random = new();

    public static ProductEntity CreateProduct(Guid supplierId, int supplierNumber, int productNumber)
    {
        return new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = $"Product {supplierNumber}-{productNumber}",
            Weight = Math.Round(Random.NextDouble() * 1000, 0),
            Price = Math.Round((decimal)(Random.NextDouble() * 300), 2),
            SupplierId = supplierId
        };
    }
}