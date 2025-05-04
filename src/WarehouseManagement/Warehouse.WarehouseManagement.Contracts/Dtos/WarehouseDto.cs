using Warehouse.Core.Dtos;

namespace Warehouse.WarehouseManagement.Contracts.Dtos;

public class WarehouseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public SizeDto Size { get; set; }
    
    public FullSectionDto[] Sections { get; set; }
}