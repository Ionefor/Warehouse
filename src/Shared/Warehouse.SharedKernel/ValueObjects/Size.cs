using CSharpFunctionalExtensions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehouse.SharedKernel.ValueObjects;

public class Size : ComparableValueObject
{
    private Size(double length, double width, double height)
    {
        Length = length;
        Width = width;
        Height = height;
    }
    
    public double Height { get; }
    
    public double Width { get; }
    
    public double Length { get; }
    
    public static Result<Size, Error> Create(double length, double width, double height)
    {
        if (length > Constants.Size.MaxLengthSize || length < Constants.Size.MinSize ||
            width > Constants.Size.MaxWidthSize || width < Constants.Size.MinSize ||
            height > Constants.Size.MaxHeightSize || height < Constants.Size.MinSize)
        {
            return Errors.General.
                ValueIsInvalid(new ErrorParameters.ValueIsInvalid(nameof(Size)));
        }
        
        return new Size(length, width, height);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Height;
        yield return Width;
        yield return Length;
    }
}