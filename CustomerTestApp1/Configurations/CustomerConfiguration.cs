using CustomerTestApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CustomerTestApp1.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CostumerId);
            builder.HasIndex(c => c.Email).IsUnique();

            builder.Property(c => c.Email).IsRequired().HasMaxLength(255);

            builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);

            builder.Property(c => c.DateCreated).HasDefaultValueSql("CURRENT_TIMESTAMP");

        }
    }
}
