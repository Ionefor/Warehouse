using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehourse.ProductPlacement.Domain.ValueObjects;

public class ShelfPosition : ComparableValueObject
{
    private ShelfPosition() {}

    private ShelfPosition(
        int sectionRowNumber,
        int shelfRowNumber,
        int shelfColumnNumber)
    {
        SectionRowNumber = sectionRowNumber;
        ShelfRowNumber = shelfRowNumber;
        ShelfColumnNumber = shelfColumnNumber;
    }
    
    public int SectionRowNumber { get; }
    public int ShelfRowNumber { get; }
    public int ShelfColumnNumber { get; }
    
    public static Result<ShelfPosition, Error> Create(
        int sectionRowNumber,
        int shelfRowNumber,
        int shelfColumnNumber)
    {
        // if (string.IsNullOrWhiteSpace(sectionRowNumber))
        // {
        //     return Errors.General.
        //         ValueIsInvalid(new ErrorParameters.ValueIsInvalid(nameof(ShelfPosition)));
        // }
        
        return new ShelfPosition(
            sectionRowNumber,
            shelfRowNumber,
            shelfColumnNumber);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return SectionRowNumber;
        yield return ShelfRowNumber;
        yield return ShelfColumnNumber;
    }
}