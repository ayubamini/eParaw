using MediatR;


namespace Catalog.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<List<CategoryDto>>
    {
        public bool ActiveOnly { get; init; } = true;
    }
}
