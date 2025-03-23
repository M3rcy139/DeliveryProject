using FluentValidation;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Dto;

namespace DeliveryProject.Core.Validators
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().When(x => x.OrderId.HasValue)
                .WithMessage(ValidationErrorMessages.RequiredOrderId);

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage(ValidationErrorMessages.RequiredCustomerId);

            RuleFor(x => x.Products)
                .NotEmpty().WithMessage(ValidationErrorMessages.RequiredProducts)
                .Must(p => p.Count > 0).WithMessage(ValidationErrorMessages.ProductListNotEmpty);
        }
    }
}
