namespace Catalog.Application.Features.Products.Queries.GetProducts
{
    using Catalog.Application.Common.Models;
    using MediatR;

    public record GetProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string SearchTerm { get; init; }
        public int? CategoryId { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public bool? InStockOnly { get; init; }
    }

}