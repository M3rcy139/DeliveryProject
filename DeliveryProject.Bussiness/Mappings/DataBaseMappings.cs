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
            CreateMap<AttributeEntity, Core.Models.Attribute>();
            CreateMap<DeliverySlotEntity, DeliverySlot>();
            CreateMap<InvoiceEntity, Invoice>();
            CreateMap<OrderProductEntity, OrderProduct>();

            CreateMap<PersonEntity, Person>()
                .Include<CustomerEntity, Customer>()
                .Include<DeliveryPersonEntity, DeliveryPerson>()
                .Include<SupplierEntity, Supplier>();

            CreateMap<CustomerEntity, Customer>();
            CreateMap<DeliveryPersonEntity, DeliveryPerson>();
            CreateMap<SupplierEntity, Supplier>();

            CreateMap<AttributeValueEntity, AttributeValue>();
            CreateMap<ProductEntity, Product>();
            CreateMap<RegionEntity, Region>();
            CreateMap<RoleEntity, Role>();
            CreateMap<RoleAttributeEntity, RoleAttribute>();

            CreateMap<OrderPersonEntity, OrderPerson>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person.ToPerson()));

            CreateMap<OrderEntity, Order>()
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
