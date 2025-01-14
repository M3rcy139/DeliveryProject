using FluentValidation;
using DeliveryProject.Core.Constants;

namespace DeliveryProject.API.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task<bool> TryValidateAsync<T>(
            this IValidator<T> validator, T instance)
        {
            var validationResult = await validator.ValidateAsync(instance);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(string.Format(ErrorMessages.Validation.ValidationFailed, 
                    validationResult.Errors), validationResult.Errors);
            }

            return true;
        }
    }
}
