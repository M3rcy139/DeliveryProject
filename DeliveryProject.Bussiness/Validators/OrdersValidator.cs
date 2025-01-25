using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class OrdersValidator : AbstractValidator<List<OrderEntity>>
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
