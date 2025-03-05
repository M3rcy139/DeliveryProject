using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Tests.Helpers
{
    public static class PersonContactHelper
    {
        public static PersonContactEntity CreatePersonContact(Guid personId, Random random, int index, string emailPrefix)
        {
            return new PersonContactEntity
            {
                Id = Guid.NewGuid(),
                PhoneNumber = $"{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                Email = $"{emailPrefix}{index}@example.com",
                RegionId = random.Next(1, 80),
                PersonId = personId
            };
        }
    }
}
