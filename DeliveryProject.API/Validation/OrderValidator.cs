using FluentValidation;
using DeliveryProject.Core.Models;

namespace DeliveryProject.API.Validation
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage("The RegionId must be greater than zero.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("The weight of the order must be positive.");

            RuleFor(x => x.DeliveryTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("The delivery time cannot be in the past.");
        }
    }
}
