using CSharpFunctionalExtensions;

namespace Warehouse.WarehouseManagement.Domain.ValueObjects;

public class SectionRows : ComparableValueObject
{
    private SectionRows() {}
    
    public SectionRows(IEnumerable<SectionRow> sectionRows)
    {
        Values = sectionRows.ToList();
    }
    
    public IReadOnlyList<SectionRow>? Values { get; }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        foreach (var rows in Values)
        {
            yield return rows;
        }
    }
}