using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Helpers
{
    public static class OrderSortingHelper
    {
        public static Func<List<OrderEntity>, List<OrderEntity>>? GetSortDelegate(OrderSortField? sortBy, bool descending)
        {
            return sortBy switch
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

        public static void ValidateOrders(List<OrderEntity> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                throw new BussinessArgumentException(ErrorMessages.Order.NotFound, ErrorCodes.Order.NoOrdersFound);
            }
        }
    }
}
