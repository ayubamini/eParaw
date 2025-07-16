using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            product.SetName(request.Name);
            product.SetDescription(request.Description);
            product.SetPrice(request.Price, request.Currency);
            product.SetStock(request.Stock);

            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                product.ImageUrl = request.ImageUrl;
            }

            await _productRepository.UpdateAsync(product);
        }
    }
}

