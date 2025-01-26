using DeliveryProject.Bussiness.Enums;
using DeliveryProject.Core.Extensions;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Helpers
{
    public static class OrderServiceHelper
    {
        public static void ValidateOrders(ref List<OrderEntity> orders)
        {
            if (orders.IsNullOrEmpty())
            {
                orders = new List<OrderEntity>();
            }
        }

        public static void ValidateOrder(OrderEntity order)
        {
            if (order == null)
            {
                throw new BussinessArgumentException(ErrorMessages.OrderNotFound, ErrorCodes.NoOrdersFound);
            }
        }

        public static void ValidateOrdersInRegion(bool hasOrders, int regionId)
        {
            if (!hasOrders)
            {
                throw new BussinessArgumentException(string.Format(ErrorMessages.NoOrderInRegion, regionId),
                    ErrorCodes.NoOrdersFound);
            }
        }

        public static void ValidateRegionName(string regionName)
        {
            if (string.IsNullOrEmpty(regionName))
            {
                throw new BussinessArgumentException(ErrorMessages.RegionMustNotBeEmpty, ErrorCodes.MustNotBeEmpty);
            }
        }

        public static void ValidateRegion(RegionEntity? region, string regionName)
        {
            if (region == null)
            {
                throw new BussinessArgumentException(
                    string.Format(ErrorMessages.RegionNotFound, regionName),
                    ErrorCodes.RegionNotFound);
            }
        }

        public static void ValidateSupplier(SupplierEntity? supplier)
        {
            if (supplier == null)
            {
                throw new BussinessArgumentException(ErrorMessages.SupplierNotFound, ErrorCodes.SupplierNotFound);
            }
        }

        public static void ValidateDeliveryPerson(DeliveryPersonEntity? deliveryPerson)
        {
            if (deliveryPerson == null)
            {
                throw new BussinessArgumentException(ErrorMessages.NoAvailableDeliveryPersons,
                    ErrorCodes.NoAvailableDeliveryPersons);
            }
        }
    }
}
