using DeliveryProject.Core.Exceptions;

namespace DeliveryProject.Core.Extensions
{
    public static class EntityExtensions
    {
        public static void ValidateEntity<T>(this T entity, string errorMessage, string errorCode)
        where T : class
        {
            if (entity == null)
            {
                throw new BussinessArgumentException(errorMessage, errorCode);
            }
        }
    }
}
