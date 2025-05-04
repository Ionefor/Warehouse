using Warehouse.Core.Dtos;

namespace Warehourse.ProductPlacement.Contracts.Dtos;

public class PendingProductDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    
    public string Description { get; init; }
    
    public string Category  { get; init; }
    
    public SizeDto Size  { get; init; }
}