using FluentValidation;

namespace Catalog.Application.Features.Products.Commands.UpdateStock
{
    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            RuleFor(v => v.ProductId)
                .GreaterThan(0).WithMessage("Valid product ID is required.");

            RuleFor(v => v.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(v => v.OperationType)
                .IsInEnum().WithMessage("Valid operation type is required.");
        }
    }
}
