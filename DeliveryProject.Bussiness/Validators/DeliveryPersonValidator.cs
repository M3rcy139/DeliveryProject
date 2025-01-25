using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class DeliveryPersonValidator : AbstractValidator<DeliveryPersonEntity>
    {
        public DeliveryPersonValidator()
        {
            RuleFor(x => x).NotNull().WithMessage(ErrorMessages.NoAvailableDeliveryPersons);
        }
    }
}
