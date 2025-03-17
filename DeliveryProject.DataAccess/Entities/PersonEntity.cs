using System.ComponentModel.DataAnnotations.Schema;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Extensions;

namespace DeliveryProject.DataAccess.Entities
{
    public abstract class PersonEntity
    {
        public Guid Id { get; set; }
        public PersonStatus Status { get; set; } 
        public int RegionId { get; set; }
        public int RoleId { get; set; }

        public RegionEntity Region { get; set; }
        public RoleEntity Role { get; set; }
        public ICollection<AttributeValueEntity> AttributeValues { get; set; } 
            = new List<AttributeValueEntity>();

        [NotMapped]
        protected string? Name
        {
            get => this.GetAttributeValue(AttributeKey.Name);
            set => this.SetAttributeValue(AttributeKey.Name, value);
        }
        
        [NotMapped]
        protected string? PhoneNumber
        {
            get => this.GetAttributeValue(AttributeKey.PhoneNumber);
            set => this.SetAttributeValue(AttributeKey.PhoneNumber, value);
        }
        
        [NotMapped]
        protected string? Email
        {
            get => this.GetAttributeValue(AttributeKey.Email);
            set => this.SetAttributeValue(AttributeKey.Email, value);
        }
    }
}