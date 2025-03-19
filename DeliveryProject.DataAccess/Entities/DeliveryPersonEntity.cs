using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;


namespace DeliveryProject.DataAccess.Entities
{
    public class DeliveryPersonEntity : PersonEntity
    {
        [NotMapped]
        public double Rating
        {
            get => double.TryParse(GetAttributeValue(AttributeKey.Rating), out var rating) ? rating : 0;
            set => SetAttributeValue(AttributeKey.Rating, value.ToString());
        }
    }
}
