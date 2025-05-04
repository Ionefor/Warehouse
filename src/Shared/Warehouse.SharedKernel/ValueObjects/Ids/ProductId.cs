using Warehouse.SharedKernel.Models;

namespace Warehouse.SharedKernel.ValueObjects.Ids;

public class ProductId(Guid id) : BaseId<ProductId>(id)
{
    public static implicit operator Guid(ProductId productId) => productId.Id;
    public static implicit operator ProductId(Guid id) => new(id);
}