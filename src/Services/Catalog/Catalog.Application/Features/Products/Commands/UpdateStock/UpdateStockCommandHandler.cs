using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateStock
{
    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateStockCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            switch (request.OperationType)
            {
                case StockOperationType.Add:
                    product.AddStock(request.Quantity);
                    break;
                case StockOperationType.Remove:
                    product.RemoveStock(request.Quantity);
                    break;
                default:
                    throw new ArgumentException("Invalid stock operation type");
            }

            await _productRepository.UpdateAsync(product);
        }
    }
}
