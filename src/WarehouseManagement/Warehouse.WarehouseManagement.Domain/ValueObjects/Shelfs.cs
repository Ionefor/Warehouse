using CSharpFunctionalExtensions;

namespace Warehouse.WarehouseManagement.Domain.ValueObjects;

public class Shelfs : ComparableValueObject
{
    private Shelfs() {}
    
    public Shelfs(IEnumerable<Shelf> shelfs)
    {
        Values = shelfs.ToList();
    }
    
    public IReadOnlyList<Shelf>? Values { get; }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        foreach (var shelf in Values)
        {
            yield return shelf;
        }
    }
}