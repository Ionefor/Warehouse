using Warehouse.SharedKernel.Models;

namespace Warehouse.WarehouseManagement.Contracts.Dtos;

public class SectionViewDto
{
    public int SectionRowCount { get; set; }
    
    public int SectionRowShelfsCount { get; set; }
    
    public SectionType Type { get; set; }
    
    public string SectionFullness { get; set; }
}