using ByWay.Domain.Enums;

namespace ByWay.Domain.Entities
{
  public class Course : BaseEntity
  {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Certification { get; set; }
    public decimal Cost { get; set; }
    public CourseLevel Level { get; set; }
    public double Rate { get; set; }
    public int TotalHours { get; set; }
    public string? ImagePath { get; set; }

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<CourseContent> Contents { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];
  }
}