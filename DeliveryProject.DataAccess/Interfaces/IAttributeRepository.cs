using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IAttributeRepository
    {
        Task<Dictionary<AttributeKey, int>> GetAttributeIdsByKeys(IEnumerable<AttributeKey> keys);
    }
}
