using Warehouse.Core.Abstractions;
using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Create;

public record CreateWarehouseCommand(
    string Name,
    SizeDto WarehouseSize,
    List<SectionDto> Sections,
    string Email) : ICommand;