using ByWay.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ByWay.Domain.DTOs;

public record AddInstructorDto
{
  [Required]
  [StringLength(100)]
  public string FullName { get; init; } = string.Empty;

  [Required]
  [StringLength(1000)]
  public string Bio { get; init; } = string.Empty;

  [Required]
  public double Rate { get; init; }

  [Required]
  [EnumDataType(typeof(JobTitle), ErrorMessage = "Invalid Instructor Job Title.")]
  public JobTitle JobTitle { get; init; }

  [Required]
  public IFormFile? Image { get; init; }
}

