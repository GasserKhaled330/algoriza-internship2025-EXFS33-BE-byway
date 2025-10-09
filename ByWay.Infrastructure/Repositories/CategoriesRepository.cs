using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Infrastructure.Data.Contexts.AppContext;

namespace ByWay.Infrastructure.Repositories;

public class CategoriesRepository : Repository<Category>, ICategoriesRepository
{
  public CategoriesRepository(AppDbContext context) : base(context)
  {

  }
}