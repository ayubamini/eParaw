using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Check if category name already exists
            var existingCategory = await _categoryRepository.GetByNameAsync(request.Name);
            if (existingCategory != null)
            {
                throw new ValidationException(new[]
                {
                    new FluentValidation.Results.ValidationFailure("Name", "A category with this name already exists.")
                });
            }

            var category = new Category(request.Name, request.Description, request.ImageUrl);
            await _categoryRepository.AddAsync(category);

            return category.Id;
        }
    }
}
