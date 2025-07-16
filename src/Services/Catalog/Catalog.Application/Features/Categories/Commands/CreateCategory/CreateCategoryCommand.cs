using MediatR;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }
    }
}
