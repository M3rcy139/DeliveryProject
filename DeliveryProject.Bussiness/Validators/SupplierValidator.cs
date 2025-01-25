using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Entities;
using FluentValidation;

namespace DeliveryProject.Bussiness.Validators
{
    public class SupplierValidator : AbstractValidator<SupplierEntity>
    {
        public SupplierValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage(ErrorMessages.SupplierNotFound);
        }
    }
}
