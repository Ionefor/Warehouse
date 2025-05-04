using Warehouse.Core.Dtos;

namespace Warehouse.WarehouseManagement.Contracts.Dtos;

public record SectionRowDto(SizeDto Size, int Number, IEnumerable<ShelfDto> Shelfs);