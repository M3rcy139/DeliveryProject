using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Extensions;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Helpers;

public static class BuildEntityHelper
{
    public static OrderEntity BuildNewOrderEntity(Order order,
        CustomerEntity customer,
        List<OrderProductEntity> orderProducts,
        decimal amount)
    {
        return new OrderEntity
        {
            Id = order.Id,
            CreatedTime = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Amount = amount,
            OrderPersons = new List<OrderPersonEntity>
            {
                new OrderPersonEntity { Person = customer }
            },
            OrderProducts = orderProducts
        };
    }

    public static void BuildUpdatedOrderEntity(OrderEntity updatedOrder, List<OrderProductEntity> orderProducts,
        decimal amount)
    {
        updatedOrder.Amount = amount;
        updatedOrder.OrderProducts.Clear();
        updatedOrder.OrderProducts.AddRange(orderProducts.Select(op =>
            new OrderProductEntity
            {
                ProductId = op.ProductId,
                OrderId = op.OrderId,
                Quantity = op.Quantity
            }));
    }

    public static List<OrderProductEntity> BuildOrderProductsEntity(Order order, List<ProductEntity> productEntities,
        List<ProductDto> products)
    {
        return products
            .Select(p => new OrderProductEntity
            {
                OrderId = order.Id,
                ProductId = p.ProductId,
                Product = productEntities.First(pe => pe.Id == p.ProductId),
                Quantity = p.Quantity
            }).ToList();
    }
}