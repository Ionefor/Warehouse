using Warehouse.Core.Abstractions;

namespace Warehourse.ProductPlacement.Application.Features.Command.Product.Transfer;

public record ProductTransferCommand(
    Guid ProductId,
    Guid WarehouseId,
    Guid SectionId,
    int RowNumber,
    int ShelfRow,
    int ShelfColumn) : ICommand;