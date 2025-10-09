using ByWay.Domain.Entities;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Data.Seeders.Configuration;

namespace ByWay.Infrastructure.Data.Seeders;

public class CourseContentSeeder : BaseSeeder<CourseContent>
{


  public CourseContentSeeder(AppDbContext context) : base(context)
  {
  }
  public override async Task SeedAsync()
  {
    if (!await HasDataAsync())
    {
      var filePath = Path.Combine(SeederConfiguration.BaseDirectory, SeederConfiguration.FileNames.CourseContents);
      var courseContents = await ReadAsJsonFormatAsync(filePath);
      if (courseContents?.Count > 0)
      {
        await SaveDataAsync(courseContents);
      }
    }
  }
}