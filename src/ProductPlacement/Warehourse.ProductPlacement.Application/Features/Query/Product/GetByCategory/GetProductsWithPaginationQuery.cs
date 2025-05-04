using Warehouse.Core.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Application.Features.Query.Product.GetByCategory;

public record GetProductsWithPaginationQuery(
    PaginationParamsDto? PaginationParams, string Category) : IQuery;