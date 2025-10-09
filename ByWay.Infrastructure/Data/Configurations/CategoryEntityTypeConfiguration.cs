using ByWay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByWay.Infrastructure.Data.Configurations
{
  internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {
      ArgumentNullException.ThrowIfNull(builder);
      builder.HasKey(category => category.Id);
      builder.Property(category => category.Name).IsRequired().HasMaxLength(256);
      builder.Property(category => category.Description).IsRequired(false).HasMaxLength(500);
      builder.Property(category => category.ImagePath).IsUnicode(false).HasMaxLength(500);
    }
  }
}
