using ByWay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByWay.Infrastructure.Data.Configurations
{
  internal class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
  {
    public void Configure(EntityTypeBuilder<Course> builder)
    {
      ArgumentNullException.ThrowIfNull(builder);
      builder.HasKey(course => course.Id);
      builder.Property(course => course.Name).IsRequired().HasMaxLength(256);
      builder.Property(course => course.Description).IsRequired().HasMaxLength(1000);
      builder.Property(course => course.Certification).IsRequired(false).HasMaxLength(1000);
      builder.Property(course => course.Cost).IsRequired().HasColumnType("DECIMAL(10,2)");
      builder.Property(instructor => instructor.ImagePath).IsUnicode(false).HasMaxLength(500);
    }
  }
}
