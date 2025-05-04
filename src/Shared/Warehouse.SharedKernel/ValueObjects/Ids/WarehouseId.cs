using Warehouse.SharedKernel.Models;

namespace Warehouse.SharedKernel.ValueObjects.Ids;

public class WarehouseId(Guid id) : BaseId<WarehouseId>(id)
{
    public static implicit operator Guid(WarehouseId warehouseId) => warehouseId.Id;
    
    public static implicit operator WarehouseId(Guid id) => new(id);
}