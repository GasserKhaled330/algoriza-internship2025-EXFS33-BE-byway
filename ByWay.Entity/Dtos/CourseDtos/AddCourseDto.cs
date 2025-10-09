using ByWay.Domain.DTOs.CourseContent;
using ByWay.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ByWay.Domain.DTOs.Course;

public record AddCourseDto(
    [Required][StringLength(100)] string Name,
    [Required][StringLength(1000)] string Description,
    [StringLength(1000)] string? Certification,
    [Required]
    [RegularExpression(@"\d{1,10}(\.\d{1,2})?", ErrorMessage = "Invalid cost format.")]
    decimal Cost,
    [Required]
    [EnumDataType(typeof(CourseLevel), ErrorMessage = "Invalid Course Level.")]
    CourseLevel Level,
    [Required]
    [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5.")]
    double Rate,
    [Required]
    [Range(1, 150, ErrorMessage = "Total hours must be between 1 and 150.")]
    int TotalHours,
    [Required] IFormFile Image,
    [Required] int InstructorId,
    [Required] int CategoryId,
    [Required(ErrorMessage = "Enter at least one Course Content")]
    IEnumerable<CourseContentDto> Contents
);