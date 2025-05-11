using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Dto;
using FluentValidation;

namespace DeliveryProject.Core.Validators;

public class CreateOrderDtoValidator  : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage(ValidationErrorMessages.RequiredCustomerId);

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage(ValidationErrorMessages.RequiredProducts)
            .Must(p => p.Count > 0).WithMessage(ValidationErrorMessages.ProductListNotEmpty);
    }
}