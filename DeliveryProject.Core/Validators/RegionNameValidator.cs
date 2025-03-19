using DeliveryProject.Core.Constants.ErrorMessages;
using FluentValidation;

namespace DeliveryProject.Core.Validators
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
