using FluentValidation;
using DeliveryProject.Core.Constants.ErrorMessages;

namespace DeliveryProject.Core.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task<bool> TryValidateAsync<T>(
            this IValidator<T> validator, T instance)
        {
            if (instance == null)
            {
                return false;
            }

            var validationResult = await validator.ValidateAsync(instance);

            if (!validationResult.IsValid)
            {
                return false;
            }

            return true;
        }
    }
}
