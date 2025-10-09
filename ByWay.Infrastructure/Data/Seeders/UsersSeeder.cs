using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Data.Seeders.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ByWay.Infrastructure.Data.Seeders;

public class UsersSeeder : IDataSeeder
{
  private readonly UserManager<AppUser> _userManager;
  private static readonly JsonSerializerOptions s_readOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };
  private readonly AppDbContext _context;

  public UsersSeeder(UserManager<AppUser> userManager, AppDbContext context)
  {
    _userManager = userManager;
    _context = context;
  }

  public async Task SeedAsync()
  {
    if (!await HasDataAsync())
    {
      var filePath = Path.Combine(SeederConfiguration.BaseDirectory, SeederConfiguration.FileNames.Users);
      var json = await File.ReadAllTextAsync(filePath);
      var users = JsonSerializer.Deserialize<List<UserSeedData>>(json, s_readOptions);

      if (users is not null)
      {
        foreach (var user in users)
        {
          if (await _userManager.FindByEmailAsync(user.Email) is null)
          {
            var newUser = new AppUser
            {
              FirstName = user.FirstName,
              LastName = user.LastName,
              UserName = user.UserName,
              Email = user.Email,
              EmailConfirmed = true
            };
            await _userManager.CreateAsync(newUser, user.Password);
            await _userManager.AddToRoleAsync(newUser, user.Role);
          }
        }
      }
    }
  }

  private async Task<bool> HasDataAsync() =>
      await _context.Set<AppUser>().AnyAsync();
}

record UserSeedData(string FirstName, string LastName, string UserName, string Email, string Password, string Role);