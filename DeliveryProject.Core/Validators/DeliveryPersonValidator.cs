using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Models;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class DeliveryPersonValidator : AbstractValidator<DeliveryPerson>
    {
        public DeliveryPersonValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(BatchUploadErrorMessages.IncorrectData);
        }
    }
}
