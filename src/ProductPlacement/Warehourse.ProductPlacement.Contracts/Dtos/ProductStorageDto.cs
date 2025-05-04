namespace Warehourse.ProductPlacement.Contracts.Dtos;

public class ProductStorageDto
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Guid WarehouseId { get; set; }
    
    public Guid SectionId { get; set; }
    
    public ShelfPositionDto ShelfPosition { get; set; }
}