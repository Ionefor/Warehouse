using Microsoft.Extensions.DependencyInjection;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Contracts.Dtos;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehourse.ProductPlacement.Domain.ValueObjects;
using Warehouse.Core.Abstractions;
using Warehouse.Core.Dtos;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Contracts;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Infrastructure.Services;

public class ProductPlacementService : IProductPlacementService
{
    private readonly IProductPlacementRepository _repository;
    private readonly IWarehouseManagementContract _contract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailNotificationService _emailService;
    
    public ProductPlacementService(
        IProductPlacementRepository repository,
        IWarehouseManagementContract contract,
        IEmailNotificationService emailService,
        [FromKeyedServices(ModulesName.ProductPlacement)]IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _contract = contract;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task PendingProductPlacement(CancellationToken cancellationToken = default)
    {
        var pendingProducts = _repository.GetAllPendingProducts();

        foreach (var pendingProduct in pendingProducts)
        {
            var pendingProductDto = new PendingProductDto
            {
                Id = pendingProduct.Id,
                Name = pendingProduct.Name.Value,
                Description = pendingProduct.Description.Value,
                Category = pendingProduct.Category.Value,
                Size = new SizeDto(
                    pendingProduct.Size.Length,
                    pendingProduct.Size.Width,
                    pendingProduct.Size.Height)
            };
            
            var warehouseIds = await _contract.
                GetAllWarehouseIds(cancellationToken);
            
            var storageDto = await _contract.
                GetStorage(pendingProductDto, warehouseIds, cancellationToken);

            if (storageDto is null)
                continue;
            
            var storage = Create(storageDto);

            var product = Create(pendingProduct);

            await _repository.AddProduct(product, cancellationToken);
            
            await _repository.AddStorage(storage, cancellationToken);
            
            _repository.Delete(pendingProduct);

            var warehouse = await _contract.
                GetWarehouse(storage.WarehouseId, cancellationToken);

            await _emailService.SendNotification(product, warehouse);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
    
    private ProductStorage Create(ProductStorageDto storage)
    {
        var storageId = ProductStorageId.NewGuid();
        
        var productId = ProductId.Create(storage.ProductId);
        
        var warehouseId = WarehouseId.Create(storage.WarehouseId);
        
        var sectionId = SectionId.Create(storage.SectionId);
        
        var shelfPosition = ShelfPosition.Create(
            storage.ShelfPosition.SectionRowNumber,
            storage.ShelfPosition.ShelfRowNumber,
            storage.ShelfPosition.ShelfColumnNumber).Value;

        return new ProductStorage(storageId, productId, warehouseId, sectionId, shelfPosition);
    }
    
    private Product Create(PendingProduct product)
    {
        var productId = ProductId.Create(product.Id);

        var name = Name.Create(product.Name.Value).Value;
        
        var description = Description.Create(product.Description.Value).Value;
        
        var category = Category.Create(product.Category.Value).Value;
        
        return new Product(productId, name, description, category, product.Size);
    }
}