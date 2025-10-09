using ByWay.Domain.DTOs;
using ByWay.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace ByWay.Api.Mappers;
[Mapper(AutoUserMappings = false)]
public partial class CourseMapper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CourseMapper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    [MapProperty(nameof(Course.Instructor.FullName), nameof(CourseDto.InstructorName))]
    [MapProperty(nameof(Course.Category.Name), nameof(CourseDto.CategoryName))]
    [MapProperty(nameof(Course.ImagePath), nameof(CourseDto.ImagePath), Use = nameof(MapImagePath))]
    public partial CourseDto MapCourse(Course course);

    public partial IReadOnlyList<CourseDto> MapCourses(IReadOnlyList<Course> courses);
    
    
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