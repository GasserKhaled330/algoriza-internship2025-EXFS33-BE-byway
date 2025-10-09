namespace ByWay.Application.Specifications.CourseSpecification;

public class CourseFilterSpecification : CourseSpecification
{
  public CourseFilterSpecification(string? name = null,
      string? category = null,
      double? rating = null,
      decimal? cost = null,
      int? minLecturesCount = null,
      int? maxLecturesCount = null)
      : base
      (c => (string.IsNullOrEmpty(name) || c.Name.ToLower().Contains(name.ToLower())) &&
            (string.IsNullOrEmpty(category) || (c.Category != null && c.Category.Name.ToLower() == category.ToLower())) &&
            (!rating.HasValue || c.Rate >= rating.Value) &&
            (!cost.HasValue || c.Cost >= cost.Value) &&
            (!minLecturesCount.HasValue ||
             c.Contents.Select(content => content.LecturesCount).Sum() >= minLecturesCount.Value) &&
            (!maxLecturesCount.HasValue ||
             c.Contents.Select(content => content.LecturesCount).Sum() <= maxLecturesCount.Value)
      )
  {
  }
}