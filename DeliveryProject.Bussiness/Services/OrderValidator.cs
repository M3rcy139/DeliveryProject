using FluentValidation;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Constants.ErrorMessages;

namespace DeliveryProject.API.Services
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.CreatedTime)
                .LessThanOrEqualTo(DateTime.Now).WithMessage(ValidationErrorMessages.PastDeliveryTime);
        }

    }
}
