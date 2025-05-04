using Warehouse.Core.Abstractions;

namespace Warehouse.WarehouseManagement.Application.Features.Command.Warehouse.Delete;

public record DeleteWarehouseCommand(Guid WarehouseId) : ICommand;
