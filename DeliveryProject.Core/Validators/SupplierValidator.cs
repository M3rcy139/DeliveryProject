using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Models;
using FluentValidation;

namespace DeliveryProject.Core.Validators
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage(ErrorMessages.SupplierNotFound);
        }
    }
}
