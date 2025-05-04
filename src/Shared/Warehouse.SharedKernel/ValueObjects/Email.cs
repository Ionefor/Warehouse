using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehouse.SharedKernel.ValueObjects;

public class Email : ComparableValueObject
{
    private Email() {}
    private Email(string email)
    {
        Value = email;
    }

    public string Value { get; }
    
    public static Result<Email, Error> Create(string email)
    {
        if (!email.IsEmail())
        {
            return Errors.General.
                ValueIsInvalid(new ErrorParameters.ValueIsInvalid(nameof(Email)));
        }
        
        return new Email(email);
    }
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}