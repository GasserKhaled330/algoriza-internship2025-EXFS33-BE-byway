namespace ByWay.Domain.Entities;

public class Enrollment : BaseEntity
{
  public DateTime EnrollmentDate { get; set; } = DateTime.Now;

  public int PaymentId { get; set; }
  public Payment Payment { get; set; }
  public int CourseId { get; set; }
  public Course Course { get; set; }
  public string AppUserId { get; set; }
  public AppUser AppUser { get; set; }
}