using System.ComponentModel.DataAnnotations;

namespace ByWay.Domain.Dtos.AuthDtos;

public record RegisterDto
{
  [StringLength(50)][Required] public string FirstName { get; set; }
  [StringLength(50)][Required] public string LastName { get; set; }
  [StringLength(50)][Required] public string UserName { get; set; }
  [EmailAddress][Required] public string Email { get; set; }

  [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*])(?=.{8,}).*$",
      ErrorMessage =
          "Password must at least 8 characters, contain 1 uppercase, 1 lowercase, 1 digit, 1 special character (!@#$%^&*)")]
  [Required]
  public string Password { get; set; }

  [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
  public string ConfirmPassword { get; set; }
}