namespace DeliveryProject.Core.Dto
{
    public class DeliveryPersonDto
    {
        public string Name { get; set; }
        public double Rating { get; set; }
        public List<DeliverySlotDto> DeliverySlots { get; set; } = new();
        public List<PersonContactDto> Contacts { get; set; } = new();

        public string ToCsvString() => $"{Name},{Rating}";
    }
}
