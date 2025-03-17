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
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => MapPerson(src.Person)));

            CreateMap<OrderEntity, Order>()
                .AfterMap((src, dest) =>
                {
                    dest.OrderPersons = src.OrderPersons
                        .Select(op => new OrderPerson
                        {
                            OrderId = op.OrderId,
                            PersonId = op.PersonId,
                            Person = MapPerson(op.Person)
                        })
                        .ToList();
                });
        }

        private Person MapPerson(PersonEntity person)
        {
            switch (person)
            {
                case CustomerEntity customer:
                    return new Customer();  
                case DeliveryPersonEntity deliveryPerson:
                    return new DeliveryPerson();  
                case SupplierEntity supplier:
                    return new Supplier();  
                default:
                    throw new InvalidOperationException($"Unknown person type: {person.GetType()}");
            }
        }
    }
}
