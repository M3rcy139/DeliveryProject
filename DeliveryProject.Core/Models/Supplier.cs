using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Extensions;

namespace DeliveryProject.Core.Models
{
    public class Supplier : Person
    {
        [NotMapped]
        public double Rating
        {
            get => double.TryParse(this.GetAttributeValue(AttributeKey.Rating), out var rating) ? rating : 0;
            set => this.SetAttributeValue(AttributeKey.Rating, value.ToString());
        }
    }
}
