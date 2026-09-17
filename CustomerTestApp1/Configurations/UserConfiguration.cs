using CustomerTestApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerTestApp1.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User> {
  public void Configure(EntityTypeBuilder<User> builder) {
    builder.HasOne(u => u.Customer)
      .WithOne(c => c.User)
      .HasForeignKey<User>(u => u.CustomerId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasIndex(u => u.Email)
      .IsUnique();
    builder.HasKey(u => u.Id);

    builder.Property(u => u.Role)
      .HasConversion<string>();


    builder.Property(u => u.IsActive)
      .HasDefaultValue(true)
      .ValueGeneratedOnAdd();

    builder.Property(u => u.IsDeleted)
      .HasDefaultValue(false)
      .ValueGeneratedOnAdd();

    builder.Property(u => u.IsActive)
      .HasDefaultValue(true)
      .ValueGeneratedOnAdd();

    builder.Property(u => u.DateCreated).HasDefaultValueSql("CURRENT_TIMESTAMP");

    builder.Property(u => u.DateUpdated).HasDefaultValueSql("CURRENT_TIMESTAMP");
  }
}