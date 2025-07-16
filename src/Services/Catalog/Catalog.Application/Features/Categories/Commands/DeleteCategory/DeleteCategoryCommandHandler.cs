using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public DeleteCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            // Check if category has products
            var products = await _productRepository.GetByCategoryIdAsync(request.Id);
            if (products.Any())
            {
                throw new ValidationException(new[]
                {
                new FluentValidation.Results.ValidationFailure("", "Cannot delete category with existing products.")
            });
            }

            await _categoryRepository.DeleteAsync(category);
        }
    }

}
