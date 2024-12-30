using FluentValidation;

namespace DeliveryProject.API.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task<ValidationResult<T>> TryValidateAsync<T>(
            this IValidator<T> validator, T instance)
        {
            var validationResult = await validator.ValidateAsync(instance);

            if (validationResult.IsValid)
            {
                return ValidationResult<T>.Success(instance);
            }

            return ValidationResult<T>.Failure(validationResult.Errors);
        }
    }

}
