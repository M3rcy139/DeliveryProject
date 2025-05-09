using DeliveryProject.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DeliveryProject.Core.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task<bool> TryValidateAsync<T>(
            this IValidator<T> validator, T instance)
        {
            return instance != null && (await validator.ValidateAsync(instance)).IsValid;
        }

        public static void ValidateFile(this IFormFile file, string errorMessage)
        {
            if (file == null || file.Length == 0)
            {
                throw new BussinessArgumentException(errorMessage);
            }
        }

        public static void ValidateEntity<T>(this T entity, string errorMessage, string errorCode)
        where T : class?
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

        public static void ValidateTotalWeight(this double totalWeight, int maxTotalWeight, string errorMessage)
        {
            if (totalWeight > maxTotalWeight)
                throw new ArgumentException(string.Format(errorMessage, maxTotalWeight));
        }
    }
}
