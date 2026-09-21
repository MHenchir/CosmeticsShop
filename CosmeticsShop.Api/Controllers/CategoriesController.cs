using CosmeticsShop.Application.Categories;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var dto = await _categoryService.CreateCategoryAsync(request.Name, request.Description, cancellationToken);
        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var dtos = await _categoryService.GetAllCategoriesAsync(cancellationToken);
        return Ok(dtos);
    }
}

public sealed record CreateCategoryRequest(string Name, string? Description);