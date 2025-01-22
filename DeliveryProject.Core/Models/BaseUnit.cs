namespace DeliveryProject.Core.Models
{
    public abstract class BaseUnit
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string PhoneNumber { get; set; } 
        public double Rating { get; set; }
    }
}
