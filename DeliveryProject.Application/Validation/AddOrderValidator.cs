using FluentValidation;
using DeliveryProject.Core.Models;

namespace DeliveryProject.Application.Validation
{
    public class AddOrderValidator : AbstractValidator<Order>
    {
        public AddOrderValidator()
        {
            RuleFor(x => x.AreaId)
                .GreaterThan(0).WithMessage("AreaId должен быть больше нуля.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Вес заказа должен быть положительным.");

            RuleFor(x => x.DeliveryTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Время доставки не может быть в прошлом.");
        }
    }
}
