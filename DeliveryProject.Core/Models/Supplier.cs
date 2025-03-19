using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;


namespace DeliveryProject.Core.Models
{
    public class Supplier : Person
    {
        [NotMapped]
        public double Rating
        {
            get => double.TryParse(GetAttributeValue(AttributeKey.Rating), out var rating) ? rating : 0;
            set => SetAttributeValue(AttributeKey.Rating, value.ToString());
        }
    }
}
