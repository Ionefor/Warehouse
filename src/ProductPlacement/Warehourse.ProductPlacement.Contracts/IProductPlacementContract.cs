namespace Warehourse.ProductPlacement.Contracts;

public interface IProductPlacementContract
{
    Task PlacementProductFromWarehouse(Guid warehouseId, CancellationToken cancellationToken);
}