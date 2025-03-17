using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Extensions;

namespace DeliveryProject.DataAccess.Entities
{
    public class SupplierEntity : PersonEntity
    {
        [NotMapped]
        protected double Rating
        {
            get => double.TryParse(this.GetAttributeValue(AttributeKey.Rating), out var rating) ? rating : 0;
            set => this.SetAttributeValue(AttributeKey.Rating, value.ToString());
        }
    }
}

