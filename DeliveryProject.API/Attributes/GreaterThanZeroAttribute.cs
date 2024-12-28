using System.ComponentModel.DataAnnotations;

namespace DeliveryProject.API.Attributes
{
    public class GreaterThanZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int intValue && intValue > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Введенное значение должно быть больше нуля.");
        }
    }
}
