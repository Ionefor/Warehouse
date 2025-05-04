using FluentValidation;

namespace Warehourse.ProductPlacement.Application.Features.Command.Product.Transfer;

public class ProductTransferCommandValidator : AbstractValidator<ProductTransferCommand>
{
    public ProductTransferCommandValidator()
    {
        RuleFor(c => c.ProductId).NotEmpty().NotNull();
        
        RuleFor(c => c.WarehouseId).NotEmpty().NotNull();
        
        RuleFor(c => c.SectionId).NotEmpty().NotNull();
    }
}