using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Dto;
using FluentValidation;

namespace DeliveryProject.Core.Validators;

public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
{
    public UpdateOrderDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.RequiredOrderId);

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage(ValidationErrorMessages.RequiredProducts)
            .Must(p => p.Count > 0).WithMessage(ValidationErrorMessages.ProductListNotEmpty);
    }
}