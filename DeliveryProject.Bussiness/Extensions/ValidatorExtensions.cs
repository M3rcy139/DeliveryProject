using FluentValidation.Results;
using FluentValidation;

namespace DeliveryProject.Bussiness.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task<(bool IsValid, List<ValidationFailure> Errors)> TryValidateAsync<T>(
            this IValidator<T> validator, T instance)
        {
            var validationResult = await validator.ValidateAsync(instance);
            return (validationResult.IsValid, validationResult.Errors);
        }
    }
}
