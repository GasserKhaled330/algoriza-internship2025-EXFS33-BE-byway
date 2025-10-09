namespace ByWay.Application.Specifications.CourseSpecification;

public class CourseWithContentsSpecification : CourseSpecification
{
  public CourseWithContentsSpecification(int id)
  : base(x => x.Id == id)
  {
  }
}