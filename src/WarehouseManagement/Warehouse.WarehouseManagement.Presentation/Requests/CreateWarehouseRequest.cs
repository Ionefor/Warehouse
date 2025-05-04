using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Create;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Presentation.Requests;

public record CreateWarehouseRequest(
    string Name,
    SizeDto WarehouseSize,
    List<SectionDto> Sections, string Email)
{
    public CreateWarehouseCommand ToCommand()
        => new(Name, WarehouseSize, Sections, Email);
}