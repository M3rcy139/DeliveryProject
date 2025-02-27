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
    }
}
