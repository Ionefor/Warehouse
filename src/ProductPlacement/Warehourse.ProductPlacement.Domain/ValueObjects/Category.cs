using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehourse.ProductPlacement.Domain.ValueObjects;

public class Category : ComparableValueObject
{
    private Category() {}

    private Category(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Category, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.General.
                ValueIsInvalid(new ErrorParameters.ValueIsInvalid(nameof(Category)));
        }
        
        return new Category(value);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}