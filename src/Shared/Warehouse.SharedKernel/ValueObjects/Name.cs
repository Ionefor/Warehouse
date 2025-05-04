using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehouse.SharedKernel.ValueObjects;

public class Name : ComparableValueObject
{
    private Name() {}

    private Name(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.General.
                ValueIsInvalid(new ErrorParameters.ValueIsInvalid(nameof(Name)));
        }
        
        return new Name(value);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}