using DeliveryProject.Core.Models;

namespace DeliveryProject.Bussiness.Helpers;

public static class OrderAmountCalculator
{
    public static decimal CalculateOrderAmount(List<OrderProduct> orderProducts)
    {
        return orderProducts.Sum(op => op.Product.Price * op.Quantity);
    }
}