using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Dto;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class DeliveryPersonDtoValidator : AbstractValidator<DeliveryPersonDto>
    {
        public DeliveryPersonDtoValidator(HashSet<string> existingPhoneNumbers)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(BatchUploadErrorMessages.IncorrectData);

            RuleFor(x => x.PhoneNumber)
                .Must(phone => !existingPhoneNumbers.Contains(phone))
                .WithMessage(BatchUploadErrorMessages.AlreadyExists);
        }
    }
}
