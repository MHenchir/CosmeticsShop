using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Customers;

public sealed class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CustomerService(ICustomerRepository customerRepository, IPasswordHasher passwordHasher)
    {
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<CustomerDto?> GetCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        return customer is null ? null : MapToDto(customer);
    }

    private static CustomerDto MapToDto(Customer customer) => new(
        customer.Id, customer.FirstName, customer.LastName, customer.Email, customer.PhoneNumber);
}