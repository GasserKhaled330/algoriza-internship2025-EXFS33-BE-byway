using ByWay.Domain.Entities;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Data.Seeders.Configuration;

namespace ByWay.Infrastructure.Data.Seeders
{
  public class InstructorSeeder : BaseSeeder<Instructor>
  {
    public InstructorSeeder(AppDbContext context) : base(context)
    {
    }
    public override async Task SeedAsync()
    {

      if (!await HasDataAsync())
      {
        var filePath = Path.Combine(SeederConfiguration.BaseDirectory, SeederConfiguration.FileNames.Instructors);
        var instructors = await ReadAsJsonFormatAsync(filePath);
        if (instructors?.Count > 0)
        {
          await SaveDataAsync(instructors);
        }
      }
    }
  }
}
