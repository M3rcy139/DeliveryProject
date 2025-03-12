using CsvHelper.Configuration;
using DeliveryProject.Core.Dto;

namespace DeliveryProject.DataAccess.Mappings
{
    public class DeliveryPersonDtoMap : ClassMap<DeliveryPersonDto>
    {
        public DeliveryPersonDtoMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Rating).Name("Rating");
            Map(m => m.DeliverySlots).Convert(row => ParseDeliverySlots(row.Row.GetField("SlotTimes")));
            Map(m => m.Contacts).Convert(row => ParseContacts(row.Row.GetField("PhoneNumber"), row.Row.GetField("Email"), row.Row.GetField("RegionId"), row.Row.GetField("ExtraContacts")));
        }

        private List<DeliverySlotDto> ParseDeliverySlots(string slotTimes)
        {
            if (string.IsNullOrWhiteSpace(slotTimes)) return new List<DeliverySlotDto>();

            return slotTimes
                .Split('|')
                .Select(slot => new DeliverySlotDto { SlotTime = DateTime.Parse(slot) })
                .ToList();
        }

        private List<PersonContactDto> ParseContacts(string phoneNumber, string email, string regionId, string extraContacts)
        {
            var contacts = new List<PersonContactDto>();

            contacts.Add(new PersonContactDto
            {
                PhoneNumber = phoneNumber,
                Email = email, 
                RegionId = string.IsNullOrWhiteSpace(regionId) ? 0 : int.Parse(regionId) 
            });

            if (!string.IsNullOrWhiteSpace(extraContacts))
            {
                var extraContactList = extraContacts
                    .Split('|')
                    .Select(contact =>
                    {
                        var parts = contact.Split(':');
                        return new PersonContactDto
                        {
                            PhoneNumber = parts[0],
                            Email = parts[1], 
                            RegionId = int.Parse(parts[2]) 
                        };
                    })
                    .ToList();

                contacts.AddRange(extraContactList);
            }

            return contacts;
        }
    }
}
