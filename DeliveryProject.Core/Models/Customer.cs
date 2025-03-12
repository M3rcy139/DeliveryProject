using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Models
{
    public class Customer : Person
    {
        public string LastName { get; set; }
        public Gender Gender { get; set; }
    }
}
