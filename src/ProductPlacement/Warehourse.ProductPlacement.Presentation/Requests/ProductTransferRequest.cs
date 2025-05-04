using Warehourse.ProductPlacement.Application.Features.Command.Product.Transfer;

namespace Warehourse.ProductPlacement.Presentation.Requests;

public record ProductTransferRequest(
    Guid ProductId,
    Guid WarehouseId,
    Guid SectionId,
    int RowNumber,
    int ShelfRow,
    int ShelfColumn)
{
    public ProductTransferCommand ToCommand() =>
        new(ProductId, WarehouseId, SectionId, RowNumber, ShelfRow, ShelfColumn);
}