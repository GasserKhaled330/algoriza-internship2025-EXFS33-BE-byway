using ByWay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByWay.Infrastructure.Data.Configurations;

public class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
{
  public void Configure(EntityTypeBuilder<Payment> builder)
  {
    builder.Property(p => p.Amount)
        .HasColumnType("DECIMAL(10,2)")
        .IsRequired();

    builder.HasOne(p => p.AppUser)
        .WithMany(u => u.Payments)
        .HasForeignKey(p => p.AppUserId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}