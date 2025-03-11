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

        public static void ValidateEntities<T>(this IEnumerable<T?> entities, string errorMessage, string errorCode)
        where T : class
        {
            if (!entities.Any() || entities.Any(e => e == null))
            {
                throw new BussinessArgumentException(errorMessage, errorCode);
            }
        }
    }
}
