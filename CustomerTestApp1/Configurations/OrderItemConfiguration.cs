using CustomerTestApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerTestApp1.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem> {
  public void Configure(EntityTypeBuilder<OrderItem> builder) {
    builder.HasKey(oi => oi.Id);
    builder.Property(oi => oi.TotalNumber).HasDefaultValue(0);
    builder.Property(oi => oi.Amount).HasPrecision(18, 2).HasDefaultValue(0);

    builder.HasOne(o => o.Order)
      .WithMany(oi => oi.OrderItems)
      .HasForeignKey(o => o.OrderId);
  }
}