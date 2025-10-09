namespace ByWay.Application.Specifications.CourseSpecification;

public class TopRatedCoursesInSameCategorySpecification : CourseSpecification
{
  public TopRatedCoursesInSameCategorySpecification(int categoryId, int excludeCourseId = 0, int topCount = 3) : base(
      course => course.CategoryId == categoryId && course.Id != excludeCourseId
      )
  {
    AddOrderByDescending(c => c.Rate);
    ApplyPaging(0, topCount);
  }
}