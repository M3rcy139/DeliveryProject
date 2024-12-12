using DeliveryProject.Access.Entities;
using DeliveryProject.Core.Models;
using AutoMapper;

namespace DeliveryProject.Access.Mappings
{
    public class DataBaseMappings : Profile
    {
        public DataBaseMappings() 
        { 
            CreateMap<AreaEntity, Area>();
            CreateMap<FilteredOrderEntity, FilteredOrder>();
            CreateMap<OrderEntity, Order>();
        }
    }
}
