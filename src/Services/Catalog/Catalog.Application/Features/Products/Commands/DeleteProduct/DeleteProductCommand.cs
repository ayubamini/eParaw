// Features/Products/Commands/DeleteProduct/DeleteProductCommand.cs
namespace Catalog.Application.Features.Products.Commands.DeleteProduct;

using MediatR;

public record DeleteProductCommand(int Id) : IRequest;
