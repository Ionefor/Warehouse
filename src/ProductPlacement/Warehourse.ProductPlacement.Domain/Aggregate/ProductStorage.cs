using CSharpFunctionalExtensions;
using Warehourse.ProductPlacement.Domain.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehourse.ProductPlacement.Domain.Aggregate;

public class ProductStorage : Entity<ProductStorageId>
{
    private ProductStorage() { }
    
    public ProductStorage(
        ProductStorageId id,
        ProductId productId,
        WarehouseId warehouseId,
        SectionId sectionId,
        ShelfPosition shelfPosition) : base(id)
    {
        ProductId = productId;
        WarehouseId = warehouseId;
        SectionId = sectionId;
        ShelfPosition = shelfPosition;
    }

    public ProductId ProductId { get; init; }
    
    public WarehouseId WarehouseId { get; init; }
    
    public SectionId SectionId { get; init; }
    
    public ShelfPosition ShelfPosition { get; init; }
}