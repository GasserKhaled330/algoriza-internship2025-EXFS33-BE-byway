using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Infrastructure.Data.Contexts.AppContext;

namespace ByWay.Infrastructure.Repositories;

public class CourseContentRepository : Repository<CourseContent>, ICourseContentRepository
{
  public CourseContentRepository(AppDbContext context) : base(context)
  {
  }


}