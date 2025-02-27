using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Services
{
    public abstract class BaseService
    {
        protected static Func<List<OrderEntity>, List<OrderEntity>>? GetSortDelegate(OrderSortField? sortBy, bool descending) =>
            sortBy switch
            {
                OrderSortField.Weight => orders => descending
                    ? orders.OrderByDescending(o => o.Weight).ToList()
                    : orders.OrderBy(o => o.Weight).ToList(),
                OrderSortField.RegionId => orders => descending
                    ? orders.OrderByDescending(o => o.RegionId).ToList()
                    : orders.OrderBy(o => o.RegionId).ToList(),
                OrderSortField.DeliveryTime => orders => descending
                    ? orders.OrderByDescending(o => o.DeliveryTime).ToList()
                    : orders.OrderBy(o => o.DeliveryTime).ToList(),
                _ => null
            };
    }
}
