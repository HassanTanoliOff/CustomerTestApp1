using CustomerTestApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerTestApp1.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory> {
  public void Configure(EntityTypeBuilder<Inventory> builder) {
    builder.HasKey(i => i.Id);

    builder.Property(i => i.StockQuantity)
      .HasDefaultValue(0);

    builder.Property(i => i.CreatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();

    builder.Property(i => i.UpdatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();

    builder.HasOne(i => i.Product)
      .WithOne(p => p.Inventory)
      .HasForeignKey<Inventory>(i => i.ProductId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.ToTable(t => t.HasCheckConstraint("CK_Inventory_StockQuantity_NonNegative", "\"StockQuantity\" >= 0"));
  }
}