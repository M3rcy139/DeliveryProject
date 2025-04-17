using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Models;
using DeliveryProject.Bussiness.Extensions;
using AutoMapper;

namespace DeliveryProject.Bussiness.Mappings
{
    public class DataBaseMappings : Profile
    {
        public DataBaseMappings()
        {
            CreateMap<BaseEntity, BaseModel>();


            CreateMap<PersonEntity, Person>()
                .Include<CustomerEntity, Customer>()
                .Include<DeliveryPersonEntity, DeliveryPerson>()
                .Include<SupplierEntity, Supplier>()
                .IncludeBase<BaseEntity, BaseModel>();
            
            CreateMap<CustomerEntity, Customer>()
                .IncludeBase<BaseEntity, BaseModel>();
            CreateMap<DeliveryPersonEntity, DeliveryPerson>()
                .IncludeBase<BaseEntity, BaseModel>();
            CreateMap<SupplierEntity, Supplier>()
                .IncludeBase<BaseEntity, BaseModel>();

            CreateMap<AttributeEntity, Core.Models.Attribute>();
            CreateMap<RegionEntity, Region>();
            CreateMap<RoleEntity, Role>();

            CreateMap<DeliverySlotEntity, DeliverySlot>()
                .IncludeBase<BaseEntity, BaseModel>();
            CreateMap<InvoiceEntity, Invoice>()
                .IncludeBase<BaseEntity, BaseModel>();
            
            CreateMap<AttributeValueEntity, AttributeValue>()
                .IncludeBase<BaseEntity, BaseModel>();
            CreateMap<ProductEntity, Product>()
                .IncludeBase<BaseEntity, BaseModel>();

            CreateMap<RoleAttributeEntity, RoleAttribute>();
            CreateMap<OrderPersonEntity, OrderPerson>()
                .ForMember(dest => dest.Person, 
                    opt => opt.MapFrom(src => src.Person.ToPerson()));
            CreateMap<OrderProductEntity, OrderProduct>();
            
            CreateMap<OrderEntity, Order>()
                .IncludeBase<BaseEntity, BaseModel>()
                .AfterMap((src, dest) =>
                {
                    dest.OrderPersons = src.OrderPersons
                        .Select(op => new OrderPerson
                        {
                            OrderId = op.OrderId,
                            PersonId = op.PersonId,
                            Person = op.Person.ToPerson()
                        })
                        .ToList();
                });
        }
    }
}
