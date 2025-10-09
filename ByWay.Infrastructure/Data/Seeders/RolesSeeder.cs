using ByWay.Domain.Enums;
using ByWay.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ByWay.Infrastructure.Data.Seeders;

public class RolesSeeder : IDataSeeder
{
  private readonly RoleManager<IdentityRole> _roleManager;

  public RolesSeeder(RoleManager<IdentityRole> roleManager)
  {
    _roleManager = roleManager;
  }

  public async Task SeedAsync()
  {
    foreach (var role in Enum.GetNames(typeof(UserRole)))
    {
      if (!await _roleManager.RoleExistsAsync(role))
      {
        await _roleManager.CreateAsync(new IdentityRole(role));
      }
    }
  }
}