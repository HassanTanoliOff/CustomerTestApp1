using CustomerTestApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerTestApp1.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product> {
  public void Configure(EntityTypeBuilder<Product> builder) {
    builder.Property(p => p.Name)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(p => p.Description)
      .HasDefaultValue("No Description.")
      .HasMaxLength(500);

    builder.Property(p => p.CategoryId)
      .HasDefaultValue(1);

    builder.Property(p => p.Price)
      .HasDefaultValue(0)
      .HasPrecision(18, 2);


    builder.HasMany(p => p.OrderItems)
      .WithOne(i => i.Product)
      .HasForeignKey(o => o.ProductId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(p => p.Category)
      .WithMany(c => c.Products)
      .HasForeignKey(p => p.CategoryId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}