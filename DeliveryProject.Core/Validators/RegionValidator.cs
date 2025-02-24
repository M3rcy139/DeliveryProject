using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Models;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class RegionValidator : AbstractValidator<Region>
    {
        public RegionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorMessages.RegionNotFound, x.Name));
        }
    }
}
