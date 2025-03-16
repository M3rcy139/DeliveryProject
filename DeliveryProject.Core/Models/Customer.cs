using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Extensions;

namespace DeliveryProject.Core.Models
{
    public class Customer : Person
    {
        public string? LastName
        {
            get => this.GetAttributeValue(AttributeKey.LastName);
            set => this.SetAttributeValue(AttributeKey.LastName, value);
        }
        public Sex Sex
        {
            get => Enum.Parse<Sex>(this.GetAttributeValue(AttributeKey.Sex));
            set => this.SetAttributeValue(AttributeKey.Name, value.ToString());
        }
    }
}
