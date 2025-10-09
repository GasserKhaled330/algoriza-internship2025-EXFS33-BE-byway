using ByWay.Domain.Interfaces;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace ByWay.Infrastructure.Data.Seeders
{
  public abstract class BaseSeeder<T> : IDataSeeder where T : class
  {
    protected readonly AppDbContext _context;

    protected BaseSeeder(AppDbContext context)
    {
      _context = context;
    }
    public abstract Task SeedAsync();

    protected async Task<List<T>?> ReadAsJsonFormatAsync(string fileName)
    {
      await using var fileStream = File.OpenRead(fileName);
      return await JsonSerializer.DeserializeAsync<List<T>>(fileStream);
    }
    protected async Task SaveDataAsync(ICollection<T> data)
    {
      await _context.Set<T>().AddRangeAsync(data);
      await _context.SaveChangesAsync();
    }

    protected async Task<bool> HasDataAsync() =>
      await _context.Set<T>().AnyAsync();
  }
}
