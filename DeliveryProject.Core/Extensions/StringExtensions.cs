using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Exceptions;

namespace DeliveryProject.Core.Extensions
{
    public static class StringExtensions
    {
        public static void ValidateRegionName(this string regionName)
        {
            if (string.IsNullOrEmpty(regionName))
            {
                throw new BussinessArgumentException(ErrorMessages.RegionMustNotBeEmpty, ErrorCodes.MustNotBeEmpty);
            }
        }
    }
}
