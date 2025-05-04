using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Domain.ValueObjects;

namespace Warehouse.WarehouseManagement.Domain.Entities;

public class Section : Entity<SectionId>
{
    private Section(SectionId id) : base(id) {}

    private Section() : base(new SectionId(Guid.Empty)) {}
    
    public Section(
        SectionId id,
        Size size,
        SectionRows rows) : base(id)
    {
        Rows = rows;
        Size = size;
    }
    
    public SectionType Type { get; init; }
    
    public Size Size { get; init; }
    
    public SectionRows Rows { get; private set; }

    internal void UpdateSectionRows(SectionRows rows)
    {
        Rows = rows;
    }
}