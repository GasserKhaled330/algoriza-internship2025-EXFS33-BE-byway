using System.IdentityModel.Tokens.Jwt;
using ByWay.Api.Helper;
using ByWay.Application.Utils;
using ByWay.Domain.DTOs;
using ByWay.Domain.Dtos.AuthDtos;
using ByWay.Domain.Entities;
using ByWay.Domain.Enums;
using ByWay.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ByWay.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IOptions<JWT> _jwtOptions;

    public AuthService(UserManager<AppUser> userManager, IOptions<JWT> jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
        {
            return new AuthResponse
            {
                Message = "Username is already taken",
                IsAuthenticated = false
            };
        }
        
        if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
        {
            return new AuthResponse
            {
                Message = "Email is already registered",
                IsAuthenticated = false
            };
        }
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = registerDto.UserName,
            Email = registerDto.Email
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse
            {
                Message = $"User creation failed: {errors}",
                IsAuthenticated = false
            };
        }
        await _userManager.AddToRoleAsync(user, nameof(UserRole.User));
        var jwtSecurityToken = await TokenUtil.CreateTokenAsync(user, _userManager, _jwtOptions.Value);
        return new AuthResponse
        {
            Message = "User created successfully",
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = jwtSecurityToken.ValidTo,
            UserName = user.UserName,
            Email = user.Email,
            UserId = user.Id,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        
        if (user is null || !passwordIsCorrect)
        {
            return new AuthResponse
            {
                Message = "incorrect Email or Password",
                IsAuthenticated = false
            };
        }
        
        var jwtSecurityToken = await TokenUtil.CreateTokenAsync(user, _userManager, _jwtOptions.Value);

        return new AuthResponse
        {
            Message = "Login successful",
            IsAuthenticated = true,
            ExpiresOn = jwtSecurityToken.ValidTo,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            UserName = user.UserName,
            Email = user.Email,
            UserId = user.Id,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }
}