using Microsoft.Extensions.DependencyInjection;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Contracts;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models;

namespace Warehourse.ProductPlacement.Presentation;

public class ProductPlacementContract : IProductPlacementContract
{
    private readonly IProductPlacementRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductPlacementContract(
        IProductPlacementRepository repository,
        [FromKeyedServices(ModulesName.ProductPlacement)]IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task PlacementProductFromWarehouse(
        Guid warehouseId, CancellationToken cancellationToken)
    {
        var productStorages = await _repository.
            GetProductStorages(warehouseId, cancellationToken);
        
        var productIds = productStorages.
            Select(p => p.ProductId).ToList();

        var transaction = await _unitOfWork.
            BeginTransaction(cancellationToken);

        try
        {
            foreach (var productStorage in productStorages)
            {
                var storage = await _repository.
                    GetStorage(productStorage.Id, cancellationToken);
                
                _repository.Delete(storage);
            }
            
            foreach (var id in productIds)
            {
                var product = await _repository.
                    GetProduct(id, cancellationToken);

                var pendingProduct = new PendingProduct(
                    product.Id, product.Name,
                    product.Description, product.Category, product.Size);

                await _repository.AddPendingProduct(pendingProduct, cancellationToken);
            
                _repository.Delete(product);
            } 
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
        }
        catch(Exception e)
        {
            transaction.Rollback();
        }
    }
}