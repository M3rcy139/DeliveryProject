using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Extensions;

namespace DeliveryProject.Core.Models
{
    public abstract class Person
    {
        public Guid Id { get; set; }
        public PersonStatus Status { get; set; }
        public int RegionId { get; set; }
        public int RoleId { get; set; }

        public Region Region { get; set; }
        public Role Role { get; set; }
        public ICollection<AttributeValue> AttributeValues { get; set; }
            = new List<AttributeValue>();

        [NotMapped]
        public string? Name
        {
            get => this.GetAttributeValue(AttributeKey.Name);
            set => this.SetAttributeValue(AttributeKey.Name, value);
        }

        [NotMapped]
        public string? PhoneNumber
        {
            get => this.GetAttributeValue(AttributeKey.PhoneNumber);
            set => this.SetAttributeValue(AttributeKey.PhoneNumber, value);
        }

        [NotMapped]
        public string? Email
        {
            get => this.GetAttributeValue(AttributeKey.Email);
            set => this.SetAttributeValue(AttributeKey.Email, value);
        }
    }
}
