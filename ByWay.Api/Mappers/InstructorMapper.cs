using ByWay.Domain.DTOs;
using ByWay.Domain.Entities;
using ByWay.Domain.Enums;
using Microsoft.CodeAnalysis.Operations;
using Riok.Mapperly.Abstractions;

namespace ByWay.Api.Mappers;

[Mapper(AutoUserMappings = false)]
public partial class InstructorMapper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InstructorMapper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    [MapProperty(nameof(Instructor.ImagePath), nameof(InstructorDto.ImagePath), Use = nameof(MapImagePath))]
    public partial InstructorDto MapInstructor(Instructor instructor);

    public partial IEnumerable<InstructorDto> MapInstructors(IEnumerable<Instructor> instructors);
    
    private string? MapImagePath(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return null;
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request is null)
            return null;
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        return $"{baseUrl}/{imagePath}";
    }
}