using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Domain.Specifications;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ByWay.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
  public CourseRepository(AppDbContext context) : base(context)
  {
  }

  public async Task<IReadOnlyList<Course>> GetTopRatedCoursesAsync(ISpecification<Course> spec)
  {
    return await ApplySpecification(spec).ToListAsync();
  }

  public async Task<IReadOnlyList<Course>> GetTopRatedCoursesInSameCategoryAsync(ISpecification<Course> spec)
  {
    return await ApplySpecification(spec).ToListAsync();
  }
}