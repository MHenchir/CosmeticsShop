using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Customers;

public sealed class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> RegisterCustomerAsync(
        string firstName, string lastName, string email, string? phoneNumber,
        CancellationToken cancellationToken = default)
    {
        // Empêche un doublon d'email même avant d'atteindre l'index unique en base
        var existing = await _customerRepository.GetByEmailAsync(email, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException("Un client avec cet email existe déjà.");

        var customer = new Customer(firstName, lastName, email, phoneNumber);

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(customer);
    }

    public async Task<CustomerDto?> GetCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        return customer is null ? null : MapToDto(customer);
    }

    private static CustomerDto MapToDto(Customer customer) => new(
        customer.Id, customer.FirstName, customer.LastName, customer.Email, customer.PhoneNumber);
}