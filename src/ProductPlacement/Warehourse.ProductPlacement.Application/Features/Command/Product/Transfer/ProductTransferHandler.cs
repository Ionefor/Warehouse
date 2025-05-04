using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehourse.ProductPlacement.Domain.ValueObjects;
using Warehouse.Core.Abstractions;
using Warehouse.Core.Extensions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Contracts;

namespace Warehourse.ProductPlacement.Application.Features.Command.Product.Transfer;

public class ProductTransferHandler : ICommandHandler<Guid, ProductTransferCommand>
{
    private readonly IValidator<ProductTransferCommand> _validator;
    private readonly ILogger<ProductTransferHandler> _logger;
    private readonly IProductPlacementRepository _repository;
    private readonly IWarehouseManagementContract _contract;
    private readonly IReadDbContext _readDbContext;
    private readonly IEmailNotificationService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    
    public ProductTransferHandler(
        IValidator<ProductTransferCommand> validator,
        ILogger<ProductTransferHandler> logger,
        IProductPlacementRepository repository,
        IWarehouseManagementContract contract,
        IReadDbContext readDbContext,
        IEmailNotificationService emailService,
        [FromKeyedServices(ModulesName.ProductPlacement)]IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _readDbContext = readDbContext;
        _contract = contract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ProductTransferCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.
            ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var productR = _readDbContext.Products.
            FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken).Result;
        
        var storage = _readDbContext.ProductStorages.
            FirstOrDefaultAsync(
                p => p.ProductId == command.ProductId,
                cancellationToken).Result;
        
        var shelfAvailable = await _contract.AvailableShelf(
            command.WarehouseId, command.SectionId, command.RowNumber,
            command.ShelfRow, command.ShelfColumn, productR!.Size, cancellationToken);

        if (!shelfAvailable)
        {
            return Errors.General.
                Failed(new ErrorParameters.
                    Failed(ErrorConstants.GeneralMessage.Failed)).ToErrorList();
        }
        
        var productStorage = await _repository.
            GetStorage(storage!.Id, cancellationToken);
        
        _repository.Delete(productStorage);

        var productStorageId = ProductStorageId.NewGuid();
        var productId = ProductId.Create(command.ProductId);
        var warehouseId = WarehouseId.Create(command.WarehouseId);
        var sectionId = SectionId.Create(command.SectionId);
        var shelfPosition = ShelfPosition.
            Create(command.RowNumber, command.ShelfRow, command.ShelfColumn).Value;
            
        var newProductStorage = new ProductStorage(
            productStorageId, productId, warehouseId, sectionId, shelfPosition);
        
        await _repository.AddStorage(newProductStorage, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var product = await _repository.
            GetProduct(productR.Id, cancellationToken);
        
        var warehouse = await _contract.
            GetWarehouse(command.WarehouseId, cancellationToken);

        await _emailService.SendNotification(product, warehouse);
        
        _logger.LogInformation(
            "Product with Id {Id} transfer to Warehouse with Id {WarehouseId}",
            command.WarehouseId, productR.Id);
        
        return productR.Id;
    }
}