using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;


namespace DeliveryProject.Core.Models
{
    public class Customer : Person
    {
        [NotMapped]
        public string? LastName
        {
            get => GetAttributeValue(AttributeKey.LastName);
            set => SetAttributeValue(AttributeKey.LastName, value);
        }

        [NotMapped]
        public Sex Sex
        {
            get => Enum.Parse<Sex>(GetAttributeValue(AttributeKey.Sex));
            set => SetAttributeValue(AttributeKey.Name, value.ToString());
        }
    }
}
