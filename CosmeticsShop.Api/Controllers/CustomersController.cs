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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _customerService.GetCustomerAsync(id, cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }
}
