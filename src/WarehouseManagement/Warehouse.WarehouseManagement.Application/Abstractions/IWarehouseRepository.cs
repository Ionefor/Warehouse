using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehouse.WarehouseManagement.Application.Abstractions;

public interface IWarehouseRepository
{
    Task Add(Domain.Aggregate.Warehouse warehouse, CancellationToken cancellationToken);
    
    void Delete(Domain.Aggregate.Warehouse warehouse);
    
    Task<Domain.Aggregate.Warehouse> GetWarehouse(
        WarehouseId warehouseId, CancellationToken cancellationToken);
    
    Task<List<Guid>> GetAllWarehouseIds(
        CancellationToken cancellationToken = default);
}