using ByWay.Domain.Entities;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Data.Seeders.Configuration;

namespace ByWay.Infrastructure.Data.Seeders;

public class CoursesSeeder : BaseSeeder<Course>
{
  public CoursesSeeder(AppDbContext context) : base(context)
  {
  }

  public override async Task SeedAsync()
  {
    if (!await HasDataAsync())
    {
      var filePath = Path.Combine(SeederConfiguration.BaseDirectory, SeederConfiguration.FileNames.Courses);
      var courses = await ReadAsJsonFormatAsync(filePath);
      if (courses?.Count > 0)
      {
        await SaveDataAsync(courses);
      }
    }
  }
}