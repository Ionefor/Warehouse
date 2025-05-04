namespace Warehourse.ProductPlacement.Contracts.Dtos;

public class ShelfPositionDto
{
    public int SectionRowNumber { get; init; }
    
    public int ShelfRowNumber { get; init; }
    
    public int ShelfColumnNumber { get; init; }
}