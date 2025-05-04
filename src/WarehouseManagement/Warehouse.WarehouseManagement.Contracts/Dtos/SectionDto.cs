using Warehouse.Core.Dtos;
using Warehouse.SharedKernel.Models;

namespace Warehouse.WarehouseManagement.Contracts.Dtos;

public record SectionDto(SizeDto Size, SectionType Type);


public class FullSectionDto
{
    public Guid Id { get; set; }
    
    public Guid WarehouseId { get; set; }
    
    public SectionType Type { get; set; }
    
    public SizeDto Size { get; set; }
    
    public IReadOnlyList<SectionRowDto> SectionRows { get; init; } = [];
}

    
