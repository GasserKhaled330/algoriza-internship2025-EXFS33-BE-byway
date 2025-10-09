using ByWay.Api.Helper;
using ByWay.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ByWay.Application.Utils;

public static class TokenUtil
{
  public static async Task<JwtSecurityToken> CreateTokenAsync(
      AppUser user,
      UserManager<AppUser> userManager,
      JWT jwtOptions
  )
  {
    var userClaims = await userManager.GetClaimsAsync(user);
    var userRoles = await userManager.GetRolesAsync(user);
    var roleClaims = new List<Claim>();
    roleClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

    var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            }
        .Union(userClaims)
        .Union(roleClaims);

    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
    var signingCredentials = new SigningCredentials(
        authSigningKey,
        SecurityAlgorithms.HmacSha256
    );
    var token = new JwtSecurityToken(
        issuer: jwtOptions.Issuer,
        audience: jwtOptions.Audience,
        claims: claims,
        expires: DateTime.Now.AddDays(jwtOptions.DurationInDays),
        signingCredentials: signingCredentials
    );
    return token;
  }
}