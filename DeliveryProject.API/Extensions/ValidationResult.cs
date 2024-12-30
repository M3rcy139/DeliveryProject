using FluentValidation.Results;

namespace DeliveryProject.API.Extensions
{
    public class ValidationResult<T>
    {
        public T Value { get; }
        public List<ValidationFailure> Errors { get; }
        public bool IsValid { get; }

        public ValidationResult(T value, List<ValidationFailure> errors, bool isValid)
        {
            Value = value;
            Errors = errors;
            IsValid = isValid;
        }

        public static ValidationResult<T> Success(T value)
        {
            return new ValidationResult<T>(value, new List<ValidationFailure>(), true);
        }

        public static ValidationResult<T> Failure(List<ValidationFailure> errors)
        {
            return new ValidationResult<T>(default, errors, false);
        }
    }

}
