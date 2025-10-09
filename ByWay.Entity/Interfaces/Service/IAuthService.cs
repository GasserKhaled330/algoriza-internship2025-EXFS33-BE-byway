using ByWay.Domain.Dtos.AuthDtos;
using ByWay.Domain.DTOs;

namespace ByWay.Domain.Interfaces.Service;

public interface IAuthService
{
  Task<AuthResponse> RegisterAsync(RegisterDto registerDto);
  Task<AuthResponse> LoginAsync(LoginDto loginDto);
}