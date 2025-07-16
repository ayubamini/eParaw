using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand : IRequest
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public string Currency { get; init; }
        public int Stock { get; init; }
        public string ImageUrl { get; init; }
    }
}
