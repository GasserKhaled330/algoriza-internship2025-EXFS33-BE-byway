namespace ByWay.Application.Specifications.CourseSpecification;

public class PagedCourseFilterSpecification : CourseFilterSpecification
{
  public PagedCourseFilterSpecification(
      int pageIndex,
      int pageSize,
      string? sortBy = null,
      string? name = null,
      string? category = null,
      double? rating = null,
      decimal? cost = null,
      int? minLecturesCount = null,
      int? maxLecturesCount = null)
      :
      base(name, category, rating, cost, minLecturesCount, maxLecturesCount)
  {
    ApplyPaging((pageIndex - 1) * pageSize, pageSize);
    if (string.IsNullOrEmpty(sortBy)) return;
    switch (sortBy)
    {
      case "costAsc":
        AddOrderBy(course => course.Cost);
        break;
      case "costDesc":
        AddOrderByDescending(course => course.Cost);
        break;
      case "dateAsc":
        AddOrderBy(course => course.UpdatedAt);
        break;
      case "dateDesc":
        AddOrderByDescending(course => course.UpdatedAt);
        break;
      default:
        AddOrderByDescending(course => course.Rate);
        break;
    }

  }
}