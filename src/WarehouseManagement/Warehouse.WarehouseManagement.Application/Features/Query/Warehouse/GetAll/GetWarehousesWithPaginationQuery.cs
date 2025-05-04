using Warehouse.Core.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Features.Query.Warehouse.GetAll;

public record GetWarehousesWithPaginationQuery(PaginationParamsDto? PaginationParams) : IQuery;