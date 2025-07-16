namespace Catalog.Application.Features.Products.Queries.GetProducts
{
    using AutoMapper;
    using Catalog.Application.Common.Models;
    using Catalog.Domain.Repositories;
    using MediatR;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _productRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.CategoryId,
                request.MinPrice,
                request.MaxPrice,
                request.InStockOnly);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return new PagedResult<ProductDto>(
                productDtos,
                request.PageNumber,
                request.PageSize,
                totalCount);
        }
    }
}

