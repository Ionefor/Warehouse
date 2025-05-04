using Warehouse.SharedKernel.Models;

namespace Warehouse.SharedKernel.ValueObjects.Ids;

public class CategoryId(Guid id) : BaseId<CategoryId>(id)
{
    public static implicit operator Guid(CategoryId categoryId) => categoryId.Id;
    public static implicit operator CategoryId(Guid id) => new(id);
}