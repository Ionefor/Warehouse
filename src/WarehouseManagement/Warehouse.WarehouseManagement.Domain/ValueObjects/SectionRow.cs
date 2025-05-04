using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.SharedKernel.ValueObjects;

namespace Warehouse.WarehouseManagement.Domain.ValueObjects;

public class SectionRow : ComparableValueObject
{
    private SectionRow(Size size, Shelfs shelf, int number)
    {
        Shelfs = shelf;
        Size = size;
        Number = number;
    }
    
    public Shelfs Shelfs { get; }
    
    public Size Size { get; }
    
    public int Number { get; }
    
    public static Result<SectionRow, Error> Create(Size size, Shelfs shelf, int number)
    {
        return new SectionRow(size, shelf, number);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Size;
        yield return Number;
        yield return Shelfs;
    }
}