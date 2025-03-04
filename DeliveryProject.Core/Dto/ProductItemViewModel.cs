using DeliveryProject.Core.Constants.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace DeliveryProject.Core.Dto
{
    public class ProductItemViewModel
    {
        public Guid ProductId { get; set; }
        [Range(1, 100, ErrorMessage = ValidationErrorMessages.QuantityLength)]
        public int Quantity { get; set; }  
    }
}
