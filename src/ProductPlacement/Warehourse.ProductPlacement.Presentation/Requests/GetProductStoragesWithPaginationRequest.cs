using Warehourse.ProductPlacement.Application.Features.Query.ProductStorage.GetAll;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Presentation.Requests;

public record GetProductStoragesWithPaginationRequest(PaginationParamsDto? PaginationParams)
{
    public GetProductStoragesWithPaginationQuery ToQuery() => new(PaginationParams);
}