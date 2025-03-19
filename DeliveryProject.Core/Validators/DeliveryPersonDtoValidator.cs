using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;
using FluentValidation;
using System.Globalization;

namespace DeliveryProject.Core.Validators
{
    public class DeliveryPersonDtoValidator : AbstractValidator<DeliveryPersonDto>
    {
        public DeliveryPersonDtoValidator(HashSet<string> existingPhoneNumbers)
        {
            RuleFor(x => x.Attributes)
            .Must(attrs => attrs.Any(a => a.AttributeId == (int)AttributeKey.Name) &&
                           !string.IsNullOrWhiteSpace(attrs.FirstOrDefault(a => a.AttributeId == (int)AttributeKey.Name)?.Value))
            .WithMessage(BatchUploadErrorMessages.IncorrectData);

            RuleFor(x => x.Attributes)
                .Must(attrs => attrs.Any(a => a.AttributeId == (int)AttributeKey.Rating) &&
                               double.TryParse(attrs.FirstOrDefault(a => a.AttributeId == (int)AttributeKey.Rating)?.Value,
                               NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                .WithMessage(BatchUploadErrorMessages.IncorrectData);

            RuleFor(x => x.Attributes)
                .Must(attrs => attrs.Any(a => a.AttributeId == (int)AttributeKey.PhoneNumber) &&
                               attrs.Where(a => a.AttributeId == (int)AttributeKey.PhoneNumber)
                                    .All(a => !string.IsNullOrWhiteSpace(a.Value)))
                .WithMessage(BatchUploadErrorMessages.MustHaveOneContact);

            RuleFor(x => x.Attributes)
                .Must(attrs => attrs.Where(a => a.AttributeId == (int)AttributeKey.PhoneNumber)
                                    .All(a => !existingPhoneNumbers.Contains(a.Value)))
                .WithMessage(BatchUploadErrorMessages.AlreadyExists);
        }
    }
}
