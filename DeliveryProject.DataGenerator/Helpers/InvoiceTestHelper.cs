using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataGenerator.Helpers;

public static class InvoiceTestHelper
{
    private static readonly Random Random = new();

    public static InvoiceEntity CreateInvoice(Guid orderId, Guid deliveryPersonId)
    {
        return new InvoiceEntity
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            DeliveryPersonId = deliveryPersonId,
            DeliveryTime = new DateTime(2027, 10, 21, Random.Next(8, 11), Random.Next(0, 60), 0, DateTimeKind.Utc),
            IsExecuted = false
        };
    }
}