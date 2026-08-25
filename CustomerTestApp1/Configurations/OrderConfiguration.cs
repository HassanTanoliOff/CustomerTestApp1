using CustomerTestApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerTestApp1.Configurations
{

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(or => or.OrderId);
            builder.Property(or => or.TotalAmount).HasPrecision(18, 2).IsRequired().HasDefaultValue(0);
            builder.Property(or => or.Status).HasConversion<string>().HasMaxLength(20).HasDefaultValue(OrderStatus.Pending);
            builder.Property(or => or.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(or => or.UpdatedAt).HasDefaultValueSql("now()");
            builder.HasOne(or => or.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(or => or.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(or => or.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
