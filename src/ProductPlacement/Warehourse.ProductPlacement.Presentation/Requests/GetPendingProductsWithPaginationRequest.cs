using Warehourse.ProductPlacement.Application.Features.Query.PendingProduct.GetAll;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Presentation.Requests;

public record GetPendingProductsWithPaginationRequest(PaginationParamsDto? PaginationParamsDto)
{
    public GetPendingProductsWithPaginationQuery ToQuery()
        => new(PaginationParamsDto);
}