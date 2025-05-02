using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Business.DomainServices;

public class ProductDomainService
{
    private readonly IProductRepository _productRepository;

    public ProductDomainService(IProductRepository productRepository)
        => _productRepository = productRepository;
    
    public async Task<List<ProductEntity>> GetProductsByIds(List<Guid> ids)
    {
        var products = await _productRepository.GetProductsById(ids);
        products.ValidateEntities(ErrorMessages.ProductNotFound, ErrorCodes.ProductNotFound);
        
        return products!;
    }
}