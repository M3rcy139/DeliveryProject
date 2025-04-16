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
            CreateMap<BaseEntity, Base>();


            CreateMap<PersonEntity, Person>()
                .Include<CustomerEntity, Customer>()
                .Include<DeliveryPersonEntity, DeliveryPerson>()
                .Include<SupplierEntity, Supplier>()
                .IncludeBase<BaseEntity, Base>();
            
            CreateMap<CustomerEntity, Customer>()
                .IncludeBase<BaseEntity, Base>();
            CreateMap<DeliveryPersonEntity, DeliveryPerson>()
                .IncludeBase<BaseEntity, Base>();
            CreateMap<SupplierEntity, Supplier>()
                .IncludeBase<BaseEntity, Base>();

            CreateMap<AttributeEntity, Core.Models.Attribute>();
            CreateMap<RegionEntity, Region>();
            CreateMap<RoleEntity, Role>();

            CreateMap<DeliverySlotEntity, DeliverySlot>()
                .IncludeBase<BaseEntity, Base>();
            CreateMap<InvoiceEntity, Invoice>()
                .IncludeBase<BaseEntity, Base>();
            
            CreateMap<AttributeValueEntity, AttributeValue>()
                .IncludeBase<BaseEntity, Base>();
            CreateMap<ProductEntity, Product>()
                .IncludeBase<BaseEntity, Base>();

            CreateMap<RoleAttributeEntity, RoleAttribute>();
            CreateMap<OrderPersonEntity, OrderPerson>()
                .ForMember(dest => dest.Person, 
                    opt => opt.MapFrom(src => src.Person.ToPerson()));
            CreateMap<OrderProductEntity, OrderProduct>();
            
            CreateMap<OrderEntity, Order>()
                .IncludeBase<BaseEntity, Base>()
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
