using ByWay.Domain.Interfaces;

namespace ByWay.Application.Services;

public class DatabaseInitializer
{
  private readonly IEnumerable<IDataSeeder> _dataSeeders;

  public DatabaseInitializer(IEnumerable<IDataSeeder> dataSeeders)
  {
    _dataSeeders = dataSeeders;
  }

  public async Task InitializeAsync()
  {
    foreach (var seeder in _dataSeeders)
    {
      await seeder.SeedAsync();
    }
  }
}