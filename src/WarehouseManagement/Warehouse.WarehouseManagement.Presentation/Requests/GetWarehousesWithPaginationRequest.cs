using Warehouse.WarehouseManagement.Application.Features.Query.Warehouse.GetAll;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Presentation.Requests;

public record GetWarehousesWithPaginationRequest(PaginationParamsDto? PaginationParams)
{
    public GetWarehousesWithPaginationQuery ToQuery() => new(PaginationParams);
}