namespace Catalog.Application.Features.Products.Queries.GetProducts
{
    using AutoMapper;
    using global::Catalog.Application.Common.Mappings;
    using global::Catalog.Domain.Entities;

    public class ProductDto : IMapFrom<Product>
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
        public bool IsInStock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>()
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Price.Currency))
                .ForMember(d => d.FormattedPrice, opt => opt.MapFrom(s => s.Price.ToString()))
                .ForMember(d => d.IsInStock, opt => opt.MapFrom(s => s.IsInStock()))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty));
        }
    }
    public class ProductImageDto : IMapFrom<ProductImage>
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }
        public bool IsMain { get; set; }
    }

}

