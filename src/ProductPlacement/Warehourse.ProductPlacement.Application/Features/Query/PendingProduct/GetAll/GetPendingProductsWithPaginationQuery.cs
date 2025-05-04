using Warehouse.Core.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Application.Features.Query.PendingProduct.GetAll;

public record GetPendingProductsWithPaginationQuery(
    PaginationParamsDto? PaginationParams) : IQuery;