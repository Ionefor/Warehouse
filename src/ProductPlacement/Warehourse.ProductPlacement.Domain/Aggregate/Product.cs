using CSharpFunctionalExtensions;
using Warehourse.ProductPlacement.Domain.ValueObjects;
using Warehouse.SharedKernel.ValueObjects;
using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehourse.ProductPlacement.Domain.Aggregate;

public class Product : Entity<ProductId>
{
    private Product() { }
    
    public Product(
        ProductId id,
        Name name,
        Description description,
        Category category,
        Size size) : base(id)
    {
        Description = description;
        Category = category;
        Name = name;
        Size = size;
    }
    
    public Name Name { get; init; }
    
    public Description Description { get; init; }
    
    public Category Category  { get; init; }
    
    public Size Size { get; init; }
}