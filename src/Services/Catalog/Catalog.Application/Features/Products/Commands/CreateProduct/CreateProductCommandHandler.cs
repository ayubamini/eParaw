// Features/Products/Commands/CreateProduct/CreateProductCommandHandler.cs

namespace Catalog.Application.Features.Products.Commands.CreateProduct;

using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Check if SKU already exists
        if (await _productRepository.SkuExistsAsync(request.Sku))
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("Sku", "A product with this SKU already exists.")
            });
        }

        // Check if category exists
        var categoryExists = await _categoryRepository.ExistsAsync(request.CategoryId);
        if (!categoryExists)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Currency,
            request.Stock,
            request.Sku,
            request.CategoryId,
            request.ImageUrl);

        await _productRepository.AddAsync(product);

        return product.Id;
    }
}