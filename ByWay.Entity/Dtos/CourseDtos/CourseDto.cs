using ByWay.Domain.DTOs.CourseContent;

namespace ByWay.Domain.DTOs;

public record CourseDto(
    int Id,
    string Name,
    string Description,
    string Certification,
    decimal Cost,
    string Level,
    double Rate,
    int TotalHours,
    string ImagePath,
    int InstructorId,
    string? InstructorName,
    int CategoryId,
    string? CategoryName,
    IReadOnlyList<CourseContentDto> Contents
);