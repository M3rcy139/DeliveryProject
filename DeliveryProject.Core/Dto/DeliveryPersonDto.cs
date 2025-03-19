using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Dto
{
    public class DeliveryPersonDto
    {
        public int RegionId { get; set; }
        public int RoleId { get; set; } = (int)RoleType.DeliveryPerson;
        public List<AttributeValueDto> Attributes { get; set; } = new();
        public List<DeliverySlotDto> DeliverySlots { get; set; } = new();

        public string ToCsvString() => string.Join(",", Attributes.Select(a => a.Value));
    }
}
