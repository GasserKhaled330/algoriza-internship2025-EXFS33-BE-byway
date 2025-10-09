using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Data.Seeders;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ByWay.Application.Services;
using ByWay.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace ByWay.Api.Startup
{
  public static class DataSeederConfig
  {
    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
      services.AddScoped<IDataSeeder, CategorySeeder>();
      services.AddScoped<IDataSeeder, InstructorSeeder>();
      services.AddScoped<IDataSeeder, CoursesSeeder>();
      services.AddScoped<IDataSeeder, CourseContentSeeder>();
      services.AddScoped<IDataSeeder, RolesSeeder>();
      services.AddScoped<IDataSeeder, UsersSeeder>();
      services.AddScoped<DatabaseInitializer>();
      
      return services;
    }
    
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
      using var scope = app.Services.CreateScope();
      var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
      await initializer.InitializeAsync();
    }
  }
}
