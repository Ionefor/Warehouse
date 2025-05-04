using Warehouse.SharedKernel.Models;

namespace Warehouse.SharedKernel.ValueObjects.Ids;

public class SectionId(Guid id) : BaseId<SectionId>(id)
{
    public static implicit operator Guid(SectionId sectionId) => sectionId.Id;
    public static implicit operator SectionId(Guid id) => new(id);
}