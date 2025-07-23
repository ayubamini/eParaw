using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .UseIdentityColumn();

            builder.Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.AltText)
                .HasMaxLength(200);

            builder.Property(i => i.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            builder.Property(i => i.UpdatedAt);

            // Indexes
            builder.HasIndex(i => i.ProductId);
            builder.HasIndex(i => new { i.ProductId, i.IsMain });
        }
    }

}
