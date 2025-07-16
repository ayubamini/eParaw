using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.Features.Products.Queries.GetProductById
{
    public class ProductDetailDto : IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string FormattedPrice { get; set; }
        public int Stock { get; set; }
        public string Sku { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
