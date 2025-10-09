using ByWay.Domain.Entities;
using ByWay.Domain.Specifications;

namespace ByWay.Domain.Interfaces.Repository;

public interface ICourseRepository : IRepository<Course>
{
  Task<IReadOnlyList<Course>> GetTopRatedCoursesAsync(ISpecification<Course> spec);
  Task<IReadOnlyList<Course>> GetTopRatedCoursesInSameCategoryAsync(ISpecification<Course> spec);
}