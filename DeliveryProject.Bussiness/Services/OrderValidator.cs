using FluentValidation;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Constants.ErrorMessages;

namespace DeliveryProject.API.Services
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage(ValidationErrorMessages.InvalidRegionId);

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage(ValidationErrorMessages.InvalidWeight);

            RuleFor(x => x.DeliveryTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage(ValidationErrorMessages.PastDeliveryTime);
        }

    }
}
