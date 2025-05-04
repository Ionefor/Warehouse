using Warehouse.SharedKernel.Models;

namespace Warehouse.SharedKernel.ValueObjects.Ids;

public class ProductStorageId(Guid id) : BaseId<ProductStorageId>(id)
{
    public static implicit operator Guid(ProductStorageId productStorageId) => productStorageId.Id;
    public static implicit operator ProductStorageId(Guid id) => new(id);
}