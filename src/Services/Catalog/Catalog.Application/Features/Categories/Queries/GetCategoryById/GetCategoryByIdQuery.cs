using MediatR;

namespace Catalog.Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDetailDto>;
}
