using ByWay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByWay.Infrastructure.Data.Configurations;

public class EnrollmentEntityTypeConfiguration : IEntityTypeConfiguration<Enrollment>
{
  public void Configure(EntityTypeBuilder<Enrollment> builder)
  {
    // If a Payment is deleted, all associated Enrollments must be deleted.
    builder.HasOne(e => e.Payment)
        .WithMany(p => p.Enrollments)
        .HasForeignKey(e => e.PaymentId)
        .OnDelete(DeleteBehavior.Cascade);

    // If we delete an Enrollment, we DON'T want it to try and delete the User.
    // Setting this to Restrict is critical to break potential cycles with the Payment relationship.
    builder.HasOne(e => e.AppUser)
        .WithMany(u => u.Enrollments)
        .HasForeignKey(e => e.AppUserId)
        .OnDelete(DeleteBehavior.Restrict);

    // If a Course is deleted, we MUST NOT delete the historical enrollment records (Payment history)
    // We need to preserve the record that the course was bought.
    builder.HasOne(e => e.Course)
        .WithMany(c => c.Enrollments)
        .HasForeignKey(e => e.CourseId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}