namespace Warehouse.WarehouseManagement.Contracts.Dtos;

public class WarehouseViewDto
{
    public int SectionCount { get; set; }
    
    public int LargeSectionCount { get; set; }
    
    public int MediumSectionCount { get; set; }
    
    public int SmallSectionCount { get; set; }
    
    public string Name { get; set; }
    
    public string WarehouseFullness { get; set; }
}