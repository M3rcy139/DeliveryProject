using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Helpers;

public static class OrderAmountCalculator
{
    public static decimal CalculateOrderAmount(List<OrderProductEntity> orderProducts)
    {
        return orderProducts.Sum(op => op.Product.Price * op.Quantity);
    }
}