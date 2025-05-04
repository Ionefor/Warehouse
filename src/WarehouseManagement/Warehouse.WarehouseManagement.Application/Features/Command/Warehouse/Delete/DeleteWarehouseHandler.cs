using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Warehourse.ProductPlacement.Contracts;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Application.Abstractions;

namespace Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Delete;

public class DeleteWarehouseHandler : ICommandHandler<Guid, DeleteWarehouseCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IWarehouseRepository _repository;
    private readonly IProductPlacementContract _contract;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWarehouseHandler(
        IReadDbContext readDbContext,
        IWarehouseRepository repository,
        IProductPlacementContract contract,
        [FromKeyedServices(ModulesName.WarehouseManagement)]IUnitOfWork unitOfWork)
    {
        _readDbContext = readDbContext;
        _unitOfWork = unitOfWork;
        _contract = contract;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteWarehouseCommand command,
        CancellationToken cancellationToken = default)
    {
        var warehouseExist =
            await _readDbContext.
            Warehouses.
            AnyAsync(w => w.Id == command.WarehouseId, cancellationToken);

        if (!warehouseExist)
        {
            return Errors.Extra.InvalidDeleteOperation(
                new ErrorParameters.InvalidDeleteOperation(
                    nameof(command.WarehouseId), command.WarehouseId)).ToErrorList();
        }

        var transaction = await _unitOfWork.
            BeginTransaction(cancellationToken);
        
        try
        {
            var warehouseId = WarehouseId.Create(command.WarehouseId);
        
            var warehouse = await _repository.
                GetWarehouse(warehouseId, cancellationToken);
        
            _repository.Delete(warehouse);
        
            await _contract.
                PlacementProductFromWarehouse(warehouseId, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();
            
            return warehouseId.Id;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            
            return Errors.General.Failed(
                new ErrorParameters.Failed(ErrorConstants.
                    GeneralMessage.DeleteIsInvalid)).ToErrorList();
        }
    }
}