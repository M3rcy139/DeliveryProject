using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Dto;
using FluentValidation;

namespace DeliveryProject.Core.Validators
{
    public class DeliveryPersonDtoValidator : AbstractValidator<DeliveryPersonDto>
    {
        public DeliveryPersonDtoValidator(HashSet<string> existingPhoneNumbers)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(BatchUploadErrorMessages.IncorrectData);

            RuleFor(x => x.Contacts)
                .NotEmpty().WithMessage(BatchUploadErrorMessages.MustHaveOneContact);

            RuleForEach(x => x.Contacts).ChildRules(contact =>
            {
                contact.RuleFor(c => c.PhoneNumber)
                    .NotEmpty().WithMessage(BatchUploadErrorMessages.IncorrectData)
                    .Must(phone => !existingPhoneNumbers.Contains(phone))
                    .WithMessage(BatchUploadErrorMessages.AlreadyExists);
            });
        }
    }
}
