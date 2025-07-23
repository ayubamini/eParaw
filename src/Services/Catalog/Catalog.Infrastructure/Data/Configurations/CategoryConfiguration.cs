using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Catalog.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            // Index for name uniqueness
            builder.HasIndex(c => c.Name)
                .IsUnique();

            // Index for active categories
            builder.HasIndex(c => c.IsActive);

            // Seed data
            builder.HasData(
                new { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories", ImageUrl = "/images/categories/electronics.jpg", IsActive = true, CreatedAt = DateTime.UtcNow },
                new { Id = 2, Name = "Clothing", Description = "Men's and Women's clothing", ImageUrl = "/images/categories/clothing.jpg", IsActive = true, CreatedAt = DateTime.UtcNow },
                new { Id = 3, Name = "Books", Description = "Books and educational materials", ImageUrl = "/images/categories/books.jpg", IsActive = true, CreatedAt = DateTime.UtcNow },
                new { Id = 4, Name = "Home & Garden", Description = "Home appliances and garden tools", ImageUrl = "/images/categories/home-garden.jpg", IsActive = true, CreatedAt = DateTime.UtcNow },
                new { Id = 5, Name = "Sports & Outdoors", Description = "Sports equipment and outdoor gear", ImageUrl = "/images/categories/sports.jpg", IsActive = true, CreatedAt = DateTime.UtcNow }
            );
        }
    }
}
