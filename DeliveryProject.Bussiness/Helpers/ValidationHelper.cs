using DeliveryProject.Core.Extensions;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Exceptions;

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
    }
}
