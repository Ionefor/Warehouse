using Warehouse.Core.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Features.Query.Section.GetByWarehouseId;

public record GetSectionsWithPaginationQuery(Guid WarehouseId, PaginationParamsDto? PaginationParams) : IQuery;