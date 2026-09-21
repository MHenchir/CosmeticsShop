using CosmeticsShop.Application.Customers;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(RegisterCustomerRequest request, CancellationToken cancellationToken)
    {
        var dto = await _customerService.RegisterCustomerAsync(
            request.FirstName, request.LastName, request.Email, request.PhoneNumber, cancellationToken);

        return CreatedAtAction(nameof(GetCustomer), new { id = dto.Id }, dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _customerService.GetCustomerAsync(id, cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }
}

public sealed record RegisterCustomerRequest(
    string FirstName, string LastName, string Email, string? PhoneNumber);