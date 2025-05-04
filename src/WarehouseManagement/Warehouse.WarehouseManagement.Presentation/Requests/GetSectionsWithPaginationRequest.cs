using Warehouse.WarehouseManagement.Application.Features.Query.Section.GetByWarehouseId;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Presentation.Requests;

public record GetSectionsWithPaginationRequest(PaginationParamsDto? PaginationParams)
{
    public GetSectionsWithPaginationQuery ToQuery(Guid id) => new(id, PaginationParams);
}