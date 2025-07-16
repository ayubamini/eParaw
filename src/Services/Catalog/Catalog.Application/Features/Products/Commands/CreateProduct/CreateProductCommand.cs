// Features/Products/Commands/CreateProduct/CreateProductCommand.cs
namespace Catalog.Application.Features.Products.Commands.CreateProduct;

using MediatR;

public record CreateProductCommand : IRequest<int>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; }
    public int Stock { get; init; }
    public string Sku { get; init; }
    public int CategoryId { get; init; }
    public string ImageUrl { get; init; }
}