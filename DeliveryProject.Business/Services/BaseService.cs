using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Services
{
    public abstract class BaseService
    {
        protected static Func<List<OrderEntity>, List<OrderEntity>>? GetSortDelegate(OrderSortField? sortBy, bool descending) =>
            sortBy switch
            {
                OrderSortField.Amount => orders => descending
                    ? orders.OrderByDescending(o => o.Amount).ToList()
                    : orders.OrderBy(o => o.Amount).ToList(),
                OrderSortField.CreatedTime => orders => descending
                    ? orders.OrderByDescending(o => o.CreatedTime).ToList()
                    : orders.OrderBy(o => o.CreatedTime).ToList(),
                _ => null
            };
    }
}
