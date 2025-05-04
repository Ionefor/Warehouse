using Warehourse.ProductPlacement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Abstractions;

public interface IWarehouseService
{
    public void AddWarehouse(IEnumerable<Guid> warehouseIds);

    public Guid FindWarehouse(string category);

    Task<ProductStorageDto?> FindPlacement(
        PendingProductDto product, CancellationToken cancellationToken);
}