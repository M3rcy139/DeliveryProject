using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Models;
using FluentValidation;

namespace DeliveryProject.Core.Validators
{
    public class OrdersValidator : AbstractValidator<List<Order>>
    {
        public OrdersValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage(ErrorMessages.OrderNotFound)
                .NotEmpty()
                .WithMessage(ErrorMessages.OrderNotFound);
        }
    }
}
