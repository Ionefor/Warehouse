using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehourse.ProductPlacement.Domain.ValueObjects;
using Warehouse.Core.Abstractions;
using Warehouse.Core.Extensions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehourse.ProductPlacement.Application.Features.Command.Product.Add;

public class AddProductHandler : ICommandHandler<Guid, AddProductCommand>
{
    private readonly IValidator<AddProductCommand> _validator;
    private readonly ILogger<AddProductHandler> _logger;
    private readonly IProductPlacementRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductHandler(
        IValidator<AddProductCommand> validator,
        ILogger<AddProductHandler> logger,
        IProductPlacementRepository repository,
        [FromKeyedServices(ModulesName.ProductPlacement)]IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddProductCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.
            ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        if (command.Size.Length > Constants.ShelfSize.LargeLength ||
            command.Size.Width > Constants.ShelfSize.LargeWidth ||
            command.Size.Height > Constants.ShelfSize.LargeHeight)
        {
            return Errors.General.
                ValueIsInvalid(new ErrorParameters.
                    ValueIsInvalid(nameof(command.Size))).ToErrorList();
        }
        
        var productId = ProductId.NewGuid();
        
        var nameProduct = Name.Create(command.Name).Value;
        
        var descriptionProduct = Description.Create(command.Description).Value;
        
        var categoryProduct = Category.Create(command.Category).Value;
        
        var sizeProduct = Size.Create(
            command.Size.Length, command.Size.Width, command.Size.Height).Value;
        
        var pendingProduct = new PendingProduct(
            productId, nameProduct, descriptionProduct, categoryProduct, sizeProduct);
        
        await _repository.AddPendingProduct(pendingProduct, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation(
            "Product with Id {Id} add to pendingProduct",
            pendingProduct.Id.Id);

        return pendingProduct.Id.Id;
    }
}