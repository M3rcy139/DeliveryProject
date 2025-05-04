using DeliveryProject.Core.Constants;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.Business.DomainServices;

public class CustomerDomainService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerDomainService(ICustomerRepository customerRepository)
        => _customerRepository = customerRepository;
    
    public async Task<CustomerEntity> GetCustomerById(Guid personId)
    {
        var customer = await _customerRepository.GetCustomerById(personId);
        customer.ValidateEntity(ErrorMessages.CustomerNotFound, ErrorCodes.CustomerNotFound);

        return customer!;
    }
}