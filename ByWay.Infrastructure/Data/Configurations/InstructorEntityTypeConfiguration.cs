using ByWay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByWay.Infrastructure.Data.Configurations
{
  internal class InstructorEntityTypeConfiguration : IEntityTypeConfiguration<Instructor>
  {
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
      ArgumentNullException.ThrowIfNull(builder);
      builder.HasKey(instructor => instructor.Id);
      builder.Property(instructor => instructor.FullName).HasMaxLength(100);
      builder.Property(instructor => instructor.Bio).IsRequired().HasMaxLength(1000);
      builder.Property(instructor => instructor.ImagePath).IsUnicode(false).HasMaxLength(500);

    }
  }
}
