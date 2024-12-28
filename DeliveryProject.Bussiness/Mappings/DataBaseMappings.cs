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
            CreateMap<FilteredOrderEntity, FilteredOrder>();
            CreateMap<OrderEntity, Order>();
        }
    }
}
