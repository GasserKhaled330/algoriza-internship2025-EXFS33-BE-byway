using ByWay.Domain.Entities;
using System.Linq.Expressions;

namespace ByWay.Application.Specifications.CourseSpecification;

public class CourseSpecification : BaseSpecification<Course>
{
  protected CourseSpecification(Expression<Func<Course, bool>> criteria) : base(criteria)
  {
    AddInclude(course => course.Instructor);
    AddInclude(course => course.Category);
    AddInclude(c => c.Contents);
  }
}