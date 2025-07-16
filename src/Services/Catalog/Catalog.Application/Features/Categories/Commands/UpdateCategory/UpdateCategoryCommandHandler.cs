using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            // Check if new name conflicts with existing category
            if (category.Name != request.Name)
            {
                var existingCategory = await _categoryRepository.GetByNameAsync(request.Name);
                if (existingCategory != null && existingCategory.Id != request.Id)
                {
                    throw new ValidationException(new[]
                    {
                    new FluentValidation.Results.ValidationFailure("Name", "A category with this name already exists.")
                });
                }
            }

            category.SetName(request.Name);
            category.SetDescription(request.Description);
            category.SetImageUrl(request.ImageUrl);

            await _categoryRepository.UpdateAsync(category);
        }
    }
}
