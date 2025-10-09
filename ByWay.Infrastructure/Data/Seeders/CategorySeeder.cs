using ByWay.Domain.Entities;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Data.Seeders.Configuration;

namespace ByWay.Infrastructure.Data.Seeders
{
  public class CategorySeeder : BaseSeeder<Category>
  {
    public CategorySeeder(AppDbContext context) : base(context)
    {
    }
    public override async Task SeedAsync()
    {

      if (!await HasDataAsync())
      {
        var filePath = Path.Combine(SeederConfiguration.BaseDirectory, SeederConfiguration.FileNames.Categories);
        var categories = await ReadAsJsonFormatAsync(filePath);
        if (categories?.Count > 0)
        {
          await SaveDataAsync(categories);
        }
      }
    }
  }
}
