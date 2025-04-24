using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Extensions;

public static class OrderProductsExtensions
{
    public static decimal CalculateOrderAmount(this List<OrderProductEntity> orderProducts)
    {
        return orderProducts.Sum(op => op.Product.Price * op.Quantity);
    }
}