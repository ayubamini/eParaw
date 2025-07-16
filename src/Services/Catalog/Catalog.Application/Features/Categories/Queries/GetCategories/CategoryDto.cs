using AutoMapper;
using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.Features.Categories.Queries.GetCategories
{
    public class CategoryDto : IMapFrom<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ProductCount, opt => opt.MapFrom(s => s.Products.Count));
        }
    }
}
