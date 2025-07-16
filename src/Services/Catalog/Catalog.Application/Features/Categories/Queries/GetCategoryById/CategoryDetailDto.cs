using AutoMapper;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Features.Products.Queries.GetProducts;
using Catalog.Domain.Entities;

namespace Catalog.Application.Features.Categories.Queries.GetCategoryById
{
    public class CategoryDetailDto : IMapFrom<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ProductDto> Products { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryDetailDto>();
        }
    }
}
