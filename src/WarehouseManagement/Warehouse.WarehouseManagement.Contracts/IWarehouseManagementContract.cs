using Warehourse.ProductPlacement.Contracts.Dtos;
using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Contracts;

public interface IWarehouseManagementContract
{
    Task<ProductStorageDto?> GetStorage(
        PendingProductDto product,
        IEnumerable<Guid> warehouseIds,
        CancellationToken cancellationToken = default);
    
    Task<List<Guid>> GetAllWarehouseIds(CancellationToken cancellationToken = default);
    
    Task<WarehouseDto> GetWarehouse(Guid warehouse, CancellationToken cancellationToken = default);
    
    Task<bool> AvailableShelf(
        Guid warehouseId,
        Guid sectionId,
        int rowNumber,
        int shelfRow,
        int shelfColumn,
        SizeDto productSize,
        CancellationToken cancellationToken);
}