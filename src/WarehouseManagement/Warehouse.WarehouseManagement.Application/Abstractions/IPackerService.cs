using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Abstractions;

public interface IPackerService
{
    bool TryPack(
        SizeDto warehouseSize,
        List<SectionDto> sections);
}