using ByWay.Domain.DTOs.Category;
using ByWay.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace ByWay.Api.Mappers;

[Mapper(AutoUserMappings = false)]
public partial class CategoryMapper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CategoryMapper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    [MapProperty(nameof(Category.ImagePath), nameof(CategoryDto.ImagePath), Use = nameof(MapImagePath))]
    public partial CategoryDto MapCategory(Category category);
    public partial IReadOnlyList<CategoryDto> MapCategories(IReadOnlyList<Category> categories);
    
    
    private string? MapImagePath(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return null;
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request is null)
            return null;
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        return $"{baseUrl}/{(imagePath.StartsWith('/') ? imagePath[1..] : imagePath)}";
    }
}