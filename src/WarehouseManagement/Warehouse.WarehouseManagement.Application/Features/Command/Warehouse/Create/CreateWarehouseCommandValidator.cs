using FluentValidation;
using Warehouse.Core.Validations;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.WarehouseManagement.Domain.Entities;
using Warehouse.WarehouseManagement.Domain.ValueObjects;

namespace Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Create;

public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(c => c.Name).
            MustBeValueObject(Name.Create);
        
        RuleFor(c => c.Email).
            MustBeValueObject(Email.Create);
        
        RuleFor(c => c.WarehouseSize).
            MustBeValueObject(c => Size.Create(c.Length, c.Width, c.Height));
        
       RuleForEach(c => c.Sections).
           MustBeValueObject(s =>
               Size.Create(s.Size.Length, s.Size.Width, s.Size.Height));
    }
}