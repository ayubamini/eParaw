namespace Catalog.Application.Features.Products.Queries.GetProductById
{
    using MediatR;

    public record GetProductByIdQuery(int Id) : IRequest<ProductDetailDto>;

}

