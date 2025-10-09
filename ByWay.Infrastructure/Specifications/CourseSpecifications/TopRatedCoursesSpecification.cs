namespace ByWay.Application.Specifications.CourseSpecification;

public class TopRatedCoursesSpecification : CourseSpecification
{
  public TopRatedCoursesSpecification(int topCount = 3) : base(
      course =>
          course.Rate > 0
      )
  {
    AddOrderByDescending(c => c.Rate);
    ApplyPaging(0, topCount);
  }
}