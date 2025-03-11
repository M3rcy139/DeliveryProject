using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Models;
using AutoMapper;

namespace DeliveryProject.Bussiness.Mappings
{
    public class DataBaseMappings : Profile
    {
        public DataBaseMappings() 
        {
            CreateMap<AttributeEntity, Core.Models.Attribute>();
            CreateMap<DeliverySlotEntity, DeliverySlot>();
            CreateMap<InvoiceEntity, Invoice>();
            CreateMap<OrderEntity, Order>();
            CreateMap<OrderPersonEntity, OrderPerson>();
            CreateMap<OrderProductEntity, OrderProduct>();            
            
            CreateMap<PersonEntity, Person>();
            
            CreateMap<AttributeValueEntity, AttributeValue>();
            CreateMap<ProductEntity, Product>();
            CreateMap<RegionEntity, Region>();
            CreateMap<RoleEntity, Role>();
            CreateMap<RoleAttributeEntity, RoleAttribute>();
            
        }
    }
}
