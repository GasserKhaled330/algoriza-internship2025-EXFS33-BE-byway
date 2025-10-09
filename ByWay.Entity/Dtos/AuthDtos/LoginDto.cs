using System.ComponentModel.DataAnnotations;

namespace ByWay.Domain.Dtos.AuthDtos;

public record LoginDto(
    [Required][EmailAddress] string Email,
    [Required] string Password
);