using ByWay.Domain.Enums;

namespace ByWay.Domain.Entities
{
  public class Instructor : BaseEntity
  {
    public required string FullName { get; set; }

    public string? Bio { get; set; }

    public double Rate { get; set; }

    public JobTitle JobTitle { get; set; }

    public string? ImagePath { get; set; }

    public virtual IEnumerable<Course> Courses { get; private set; } = null!;
  }
}