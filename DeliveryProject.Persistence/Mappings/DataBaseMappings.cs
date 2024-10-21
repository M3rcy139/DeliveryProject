using DeliveryProject.Persistence.Entities;
using DeliveryProject.Core.Models;
using AutoMapper;

namespace DeliveryProject.Persistence.Mappings
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
