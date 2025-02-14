using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class RegionValidator : AbstractValidator<RegionEntity>
    {
        public RegionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorMessages.RegionNotFound, x.Name));
        }
    }
}
