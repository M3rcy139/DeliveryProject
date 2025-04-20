using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.Business.Extensions
{
    public static class MappingExtensions
    {
        public static Person ToPerson(this PersonEntity person)
        {
            switch (person)
            {
                case CustomerEntity customer:
                    return new Customer();
                case DeliveryPersonEntity deliveryPerson:
                    return new DeliveryPerson();
                case SupplierEntity supplier:
                    return new Supplier();
                default:
                    throw new InvalidOperationException(
                        string.Format(ErrorMessages.UnknownPersonType, person.GetType()));
            }
        }
    }
}
