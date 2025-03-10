using CsvHelper.Configuration;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Mappings
{
    public class DeliveryPersonDtoMap : ClassMap<DeliveryPersonDto>
    {
        public DeliveryPersonDtoMap()
        {
            Map(m => m.RegionId).Name("RegionId");
            Map(m => m.RoleId).Name("RoleId").Default((int)RoleType.DeliveryPerson);

            Map(m => m.Attributes).Convert(row =>
            {
                var attributes = new List<AttributeValueDto>();

                attributes.Add(new AttributeValueDto
                {
                    AttributeId = (int)AttributeKey.Name,
                    Value = row.Row.GetField("Name")
                });

                attributes.Add(new AttributeValueDto
                {
                    AttributeId = (int)AttributeKey.Rating,
                    Value = row.Row.GetField("Rating")
                });

                var phoneNumbers = row.Row.GetField("PhoneNumbers");
                var emails = row.Row.GetField("Emails");

                if (!string.IsNullOrWhiteSpace(phoneNumbers))
                {
                    var phones = phoneNumbers.Split('|');
                    attributes.AddRange(phones.Select(phone => new AttributeValueDto
                    {
                        AttributeId = (int)AttributeKey.PhoneNumber,
                        Value = phone
                    }));
                }

                if (!string.IsNullOrWhiteSpace(emails))
                {
                    var emailList = emails.Split('|');
                    attributes.AddRange(emailList.Select(email => new AttributeValueDto
                    {
                        AttributeId = (int)AttributeKey.Email,
                        Value = email
                    }));
                }

                return attributes;
            });

            Map(m => m.DeliverySlots).Convert(row =>
                ParseDeliverySlots(row.Row.GetField("SlotTimes")));
        }

        private List<DeliverySlotDto> ParseDeliverySlots(string slotTimes)
        {
            if (string.IsNullOrWhiteSpace(slotTimes)) return new List<DeliverySlotDto>();

            return slotTimes
                .Split('|')
                .Select(slot => new DeliverySlotDto { SlotTime = DateTime.Parse(slot) })
                .ToList();
        }
    }
}
