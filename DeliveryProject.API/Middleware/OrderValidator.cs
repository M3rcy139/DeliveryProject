using FluentValidation;
using DeliveryProject.Core.Models;

namespace DeliveryProject.API.Middleware
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage("RegionId должен быть больше нуля.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Вес заказа должен быть положительным.");

            RuleFor(x => x.DeliveryTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Время доставки не может быть в прошлом.");
        }
    }
}
