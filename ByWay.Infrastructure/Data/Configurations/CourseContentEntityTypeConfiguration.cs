using ByWay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByWay.Infrastructure.Data.Configurations
{
  internal class CourseContentEntityTypeConfiguration : IEntityTypeConfiguration<CourseContent>
  {
    public void Configure(EntityTypeBuilder<CourseContent> builder)
    {
      ArgumentNullException.ThrowIfNull(builder);
      builder.HasKey(content => content.Id);
      builder.Property(content => content.Name).IsRequired().HasMaxLength(256);
      builder.Property(content => content.LecturesCount).IsRequired();
      builder.Property(content => content.DurationInHours).IsRequired();
    }
  }
}
