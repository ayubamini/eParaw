using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateStock
{
    public record UpdateStockCommand : IRequest
    {
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public StockOperationType OperationType { get; init; }
    }

    public enum StockOperationType
    {
        Add,
        Remove
    }
}
