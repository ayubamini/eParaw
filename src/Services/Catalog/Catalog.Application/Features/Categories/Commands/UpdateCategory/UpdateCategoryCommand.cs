using MediatR;

namespace Catalog.Application.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand : IRequest
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }
    }
}
