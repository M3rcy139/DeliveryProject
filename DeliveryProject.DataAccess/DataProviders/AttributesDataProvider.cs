using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.DataProviders;

public static class AttributesDataProvider
{
    public static List<AttributeEntity> GetAttributes()
    {
        return
        [
            new AttributeEntity { Id = 1, Key = AttributeKey.Name, Type = AttributeType.String },
            new AttributeEntity { Id = 2, Key = AttributeKey.LastName, Type = AttributeType.String },
            new AttributeEntity { Id = 3, Key = AttributeKey.Sex, Type = AttributeType.String },
            new AttributeEntity { Id = 4, Key = AttributeKey.PhoneNumber, Type = AttributeType.String },
            new AttributeEntity { Id = 5, Key = AttributeKey.Email, Type = AttributeType.String },
            new AttributeEntity { Id = 6, Key = AttributeKey.Rating, Type = AttributeType.Double }
        ];
    }
}