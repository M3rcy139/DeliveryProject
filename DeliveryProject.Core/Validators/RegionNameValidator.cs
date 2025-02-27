using DeliveryProject.Core.Constants.ErrorMessages;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class RegionNameValidator : AbstractValidator<string>
    {
        public RegionNameValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage(ErrorMessages.RegionMustNotBeEmpty);
        }
    }
}
