using ByWay.Domain.Entities;

namespace ByWay.Domain.Interfaces.Service;

public interface ICategoryService
{
  Task<IReadOnlyList<Category>> GetCategoriesAsync();
}