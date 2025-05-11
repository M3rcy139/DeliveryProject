using DeliveryProject.Core.Dto;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Extensions;

public static class ProductExtensions
{
    public static double CalculateTotalWeight(
        this List<ProductEntity> productEntities,
        List<ProductDto> productDtos)
    {
        var productById = productEntities.ToDictionary(e => e.Id);

        return productDtos
            .Select(dto => productById[dto.ProductId].Weight * dto.Quantity)
            .Sum();
    }
}