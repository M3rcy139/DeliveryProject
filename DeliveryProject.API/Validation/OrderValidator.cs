using FluentValidation;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Constants;

namespace DeliveryProject.API.Validation
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage(ErrorMessages.Validation.InvalidRegionId);

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage(ErrorMessages.Validation.InvalidWeight);

            RuleFor(x => x.DeliveryTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage(ErrorMessages.Validation.PastDeliveryTime);
        }

    }
}
