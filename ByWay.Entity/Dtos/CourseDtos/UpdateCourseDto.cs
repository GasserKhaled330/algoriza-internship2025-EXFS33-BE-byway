using ByWay.Domain.DTOs.CourseContent;
using ByWay.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ByWay.Domain.Dtos.Course;

public record UpdateCourseDto(
    [StringLength(256)] string? Name,
    [StringLength(1000)] string? Description,
    [StringLength(1000)] string? Certification,
    [RegularExpression(@"\d{1,10}(\.\d{1,2})?", ErrorMessage = "Invalid cost format.")]
    decimal? Cost,
    [EnumDataType(typeof(CourseLevel), ErrorMessage = "Invalid Course Level.")]
    CourseLevel? Level,
    [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5.")]
    double? Rate,
    [Range(1, 150, ErrorMessage = "Total hours must be between 1 and 150.")]
    int? TotalHours,
    IFormFile? Image,
    int? InstructorId,
    int? CategoryId,
    ICollection<CourseContentDto>? Contents
);