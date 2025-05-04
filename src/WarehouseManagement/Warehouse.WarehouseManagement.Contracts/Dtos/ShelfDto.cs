using Warehouse.Core.Dtos;

namespace Warehouse.WarehouseManagement.Contracts.Dtos;

public record ShelfDto(bool IsAvailable, SizeDto Size, int Row, int Column);