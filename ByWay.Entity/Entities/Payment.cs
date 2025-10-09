namespace ByWay.Domain.Entities;

public class Payment : BaseEntity
{
  public decimal Amount { get; set; }
  public DateTime PaymentDate { get; set; } = DateTime.Now;

  public string AppUserId { get; set; }
  public AppUser AppUser { get; set; }

  public ICollection<Enrollment> Enrollments { get; set; } = [];
}