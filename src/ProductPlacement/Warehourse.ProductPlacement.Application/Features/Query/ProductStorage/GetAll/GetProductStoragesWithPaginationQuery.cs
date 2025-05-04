using Warehouse.Core.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Application.Features.Query.ProductStorage.GetAll;

public record GetProductStoragesWithPaginationQuery(
    PaginationParamsDto? PaginationParams) :IQuery;