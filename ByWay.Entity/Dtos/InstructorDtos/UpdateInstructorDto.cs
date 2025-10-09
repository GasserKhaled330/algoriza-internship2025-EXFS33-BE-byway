using ByWay.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ByWay.Domain.DTOs;

public record UpdateInstructorDto
{
  [StringLength(100)] public string? FullName { get; set; }
  [StringLength(1000)] public string? Bio { get; set; }
  public double? Rate { get; set; }

  [EnumDataType(typeof(JobTitle), ErrorMessage = "Invalid Instructor Job Title.")]
  public JobTitle? JobTitle { get; set; }

  public IFormFile? Image { get; set; }
}
