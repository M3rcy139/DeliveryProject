using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IAttributeValueRepository
    { 
        Task SetAttributeValue(Guid personId, AttributeKey attributeKey, string? value);
        Task<string?> GetAttributeValue(Guid personId, AttributeKey attributeKey);
    }
}
