namespace ByWay.Domain.Entities
{
  public class Category : BaseEntity
  {
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public IEnumerable<Course> Courses { get; private set; } = null!;
  }
}