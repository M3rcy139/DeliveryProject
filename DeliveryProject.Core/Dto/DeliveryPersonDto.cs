namespace DeliveryProject.Core.Dto
{
    public class DeliveryPersonDto
    {
        public string Name { get; set; } 
        public string PhoneNumber { get; set; }
        public double Rating { get; set; }

        public string ToCsvString() => $"{Name},{PhoneNumber},{Rating}";
    }
}
