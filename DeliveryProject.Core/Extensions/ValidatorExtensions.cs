using FluentValidation;

namespace DeliveryProject.Core.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task<bool> TryValidateAsync<T>(
            this IValidator<T> validator, T instance)
        {
            return instance != null && (await validator.ValidateAsync(instance)).IsValid;
        }
    }
}
