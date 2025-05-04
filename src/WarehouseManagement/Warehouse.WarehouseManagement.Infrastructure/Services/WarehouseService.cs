using Microsoft.Extensions.DependencyInjection;
using Warehourse.ProductPlacement.Contracts.Dtos;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Domain.Entities;
using Warehouse.WarehouseManagement.Domain.ValueObjects;

namespace Warehouse.WarehouseManagement.Infrastructure.Services;

public class WarehouseService : IWarehouseService
{
    private readonly ConsistentHash<Guid> _warehouseRing = new();
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IWarehouseRepository repository,
        [FromKeyedServices(ModulesName.WarehouseManagement)]IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void AddWarehouse(IEnumerable<Guid> warehouseIds)
    {
        foreach (var warehouseId in warehouseIds)
        {
            _warehouseRing.AddNode(warehouseId);
        }
    }
    
    public Guid FindWarehouse(string category)
    {
        var warehouseId = _warehouseRing.GetNode(category);

        if (warehouseId is null)
        {
            
        }
        
        return (Guid)warehouseId!;
    }

    public async Task<ProductStorageDto?> FindPlacement(
        PendingProductDto product,
        CancellationToken cancellationToken)
    {
        var warehouseId = FindWarehouse(product.Category);

        var warehouse = await _repository.GetWarehouse(warehouseId, cancellationToken);
        
        var size = Size.
            Create(product.Size.Length, product.Size.Width, product.Size.Height).Value;
        
        var section = warehouse.FindSectionBySize(size);
        
        foreach (var row in section!.Rows.Values!)
        {
            foreach (var shelf in row.Shelfs.Values!)
            {
                if (shelf.IsAvailable)
                {
                    await UpdateShelf(warehouse, section, shelf, row.Size, cancellationToken);
                    
                    return new ProductStorageDto
                    {
                        ProductId = product.Id,
                        WarehouseId = warehouseId,
                        SectionId = section.Id,
                        ShelfPosition = new ShelfPositionDto
                        {
                            SectionRowNumber = row.Number,
                            ShelfRowNumber = shelf.Row,
                            ShelfColumnNumber = shelf.Column,
                        }
                    };
                }
            }
        }

        return null;
    }

    private async Task UpdateShelf(
        Warehouse.WarehouseManagement.Domain.Aggregate.Warehouse warehouse,
        Section section, 
        Shelf oldShelf,
        Size sectionRowSize,
        CancellationToken cancellationToken)
    {
        var sectionRows = new List<SectionRow>();

        foreach (var row in section.Rows.Values!)
        {
            var shelfs = new List<Shelf>();
            
            foreach (var shelf in row.Shelfs.Values!)
            {
                if (shelf.Column == oldShelf.Column && shelf.Row == oldShelf.Row)
                {
                    shelfs.Add(Shelf.
                        Create(false, oldShelf.Size, shelf.Row, shelf.Column).Value);
                }
                else
                {
                    shelfs.Add(Shelf.
                        Create(shelf.IsAvailable, oldShelf.Size, shelf.Row, shelf.Column).Value);
                }
                
            }
            
            var sectionRow = SectionRow.
                Create(sectionRowSize, new Shelfs(shelfs), row.Number).Value;
            
            sectionRows.Add(sectionRow);
        }
        
        
        warehouse.UpdateSectionRows(section, new SectionRows(sectionRows));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}