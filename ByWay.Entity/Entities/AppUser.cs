using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ByWay.Domain.Entities;

public class AppUser : IdentityUser
{
  [Required, MaxLength(50)] public string FirstName { get; set; } = string.Empty;
  [Required, MaxLength(50)] public string LastName { get; set; } = string.Empty;
  public ICollection<Payment> Payments { get; set; } = [];
  public ICollection<Enrollment> Enrollments { get; set; } = [];
}