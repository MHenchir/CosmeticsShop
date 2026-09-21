using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Categories;

public sealed class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> CreateCategoryAsync(
        string name, string? description, CancellationToken cancellationToken = default)
    {
        var category = new Category(name, description);

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(category);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(MapToDto).ToList();
    }

    private static CategoryDto MapToDto(Category category) => new(category.Id, category.Name, category.Description);
}