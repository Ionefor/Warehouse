using FluentValidation;
using Warehourse.ProductPlacement.Domain.ValueObjects;
using Warehouse.Core.Validations;
using Warehouse.SharedKernel.ValueObjects;

namespace Warehourse.ProductPlacement.Application.Features.Command.Product.Add;


public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(c => c.Name).
            MustBeValueObject(Name.Create);
        
        RuleFor(c => c.Description).
            MustBeValueObject(Name.Create);
        
        RuleFor(c => c.Category).
            MustBeValueObject(Category.Create);
    }
}