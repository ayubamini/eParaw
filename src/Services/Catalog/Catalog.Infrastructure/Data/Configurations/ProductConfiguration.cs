using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .UseIdentityColumn();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500);

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt);

            // Value Object configuration
            builder.OwnsOne(p => p.Price, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("Price")
                    .HasPrecision(18, 2)
                    .IsRequired();

                price.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // Relationships
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(p => p.Sku)
                .IsUnique();

            builder.HasIndex(p => p.CategoryId);

            builder.HasIndex(p => p.IsActive);

            builder.HasIndex(p => new { p.Name, p.IsActive });

            // Seed data
            builder.HasData(
                new
                {
                    Id = 1,
                    Name = "Laptop Pro 15",
                    Description = "High-performance laptop with 15-inch display",
                    Stock = 50,
                    Sku = "LAPTOP-PRO-15",
                    CategoryId = 1,
                    ImageUrl = "/images/products/laptop-pro-15.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new
                {
                    Id = 2,
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with precision tracking",
                    Stock = 100,
                    Sku = "MOUSE-WIRELESS-01",
                    CategoryId = 1,
                    ImageUrl = "/images/products/wireless-mouse.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed Money value objects
            builder.OwnsOne(p => p.Price).HasData(
                new { ProductId = 1, Amount = 1299.99m, Currency = "USD" },
                new { ProductId = 2, Amount = 49.99m, Currency = "USD" }
            );
        }
    }
}
