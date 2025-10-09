namespace ByWay.Domain.Entities
{
  public class CourseContent : BaseEntity
  {
    public required string Name { get; set; }
    public int LecturesCount { get; set; }
    public int DurationInHours { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
  }
}