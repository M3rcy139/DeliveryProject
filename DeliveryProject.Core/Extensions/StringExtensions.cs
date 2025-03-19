using DeliveryProject.Core.Exceptions;

namespace DeliveryProject.Core.Extensions
{
    public static class StringExtensions
    {
        public static void ValidateNotEmpty(this string value, string errorMessage, string errorCode)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new BussinessArgumentException(errorMessage, errorCode);
            }
        }
    }
}
