using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Extensions;

namespace DeliveryProject.DataAccess.Entities
{
    public class CustomerEntity : PersonEntity
    {
        [NotMapped]
        public string? LastName
        {
            get => this.GetAttributeValue(AttributeKey.LastName);
            set => this.SetAttributeValue(AttributeKey.LastName, value);
        }

        [NotMapped]
        public Sex Sex
        {
            get => Enum.Parse<Sex>(this.GetAttributeValue(AttributeKey.Sex));
            set => this.SetAttributeValue(AttributeKey.Name, value.ToString());
        }
    }
}
