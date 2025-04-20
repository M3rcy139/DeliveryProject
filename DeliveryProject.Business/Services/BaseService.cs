using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Services
{
    public abstract class BaseService
    {
        protected static Func<List<OrderEntity>, List<OrderEntity>>? GetSortDelegate(OrderSortField? sortBy, bool descending) =>
            sortBy switch
            {
                OrderSortField.RegionId => orders => descending
                    ? orders.OrderByDescending(o => o.OrderPersons
                        .FirstOrDefault(op => op.Person.RoleId == (int)RoleType.Customer)
                        ?.Person.RegionId).ToList()
                    : orders.OrderBy(o => o.OrderPersons
                        .FirstOrDefault(op => op.Person.RoleId == (int)RoleType.Customer)
                        ?.Person.RegionId).ToList(),
                OrderSortField.CreatedTime => orders => descending
                    ? orders.OrderByDescending(o => o.CreatedTime).ToList()
                    : orders.OrderBy(o => o.CreatedTime).ToList(),
                _ => null
            };
    }
}
