using DeliveryProject.Core.Extensions;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Exceptions;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Bussiness.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateOrdersInRegion(bool hasOrders, int regionId)
        {
            if (!hasOrders)
            {
                throw new BussinessArgumentException(string.Format(ErrorMessages.NoOrderInRegion, regionId),
                    ErrorCodes.NoOrdersFound);
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

        public static void ValidateEntity<T>(T entity, string errorMessage, string errorCode)
            where T : class
        {
            if (entity == null)
            {
                throw new BussinessArgumentException(errorMessage, errorCode);
            }
        }
    }
}
