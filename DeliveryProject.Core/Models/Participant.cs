namespace DeliveryProject.Core.Models
{
    public abstract class Participant
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string PhoneNumber { get; set; } 
        public double Rating { get; set; }
    }
}
