using DeliveryProject.Core.Dto;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Extensions;

public static class ProductExtensions
{
    public static double CalculateTotalWeight(
        this List<ProductEntity> productEntities,
        List<ProductDto> productDtos)
    {
        return productDtos
            .Sum(dto => productEntities
                .FirstOrDefault(pe => pe.Id == dto.ProductId)!.Weight * dto.Quantity);
    }
}