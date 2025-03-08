using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Models;
using AutoMapper;

namespace DeliveryProject.Bussiness.Mappings
{
    public class DataBaseMappings : Profile
    {
        public DataBaseMappings() 
        { 
            CreateMap<RegionEntity, Region>();
            CreateMap<OrderEntity, Order>();
            CreateMap<DeliveryPersonEntity, DeliveryPerson>();
            CreateMap<SupplierEntity, Supplier>();
            CreateMap<PersonContactEntity, PersonContact>();
            CreateMap<RoleEntity, Role>();

            CreateMap<PersonEntity, Person>()
                .Include<CustomerEntity, Customer>()
                .Include<DeliveryPersonEntity, DeliveryPerson>()
                .Include<SupplierEntity, Supplier>();

            CreateMap<CustomerEntity, Customer>();
            CreateMap<ProductEntity, Product>();
            CreateMap<DeliverySlotEntity, DeliverySlot>();
            CreateMap<OrderProductEntity, OrderProduct>();
        }
    }
}
