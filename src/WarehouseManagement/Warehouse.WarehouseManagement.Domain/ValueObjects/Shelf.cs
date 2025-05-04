using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.SharedKernel.ValueObjects;

namespace Warehouse.WarehouseManagement.Domain.ValueObjects;

public class Shelf : ComparableValueObject
{
    public Shelf(int row, int column, Size size, bool isAvailable)
    {
        Row = row;
        Column = column;
        Size = size;
        IsAvailable = isAvailable;
    }
    
    public bool IsAvailable { get; }
    
    public Size Size { get; }
    
    public int Row { get; }
    
    public int Column { get; }
    
    public static Result<Shelf, Error> Create(bool isAvailable, Size size, int row, int column)
    {
        return new Shelf(row, column, size, isAvailable);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Row;
        yield return Column;
        yield return Size;
        yield return IsAvailable;
    }
}