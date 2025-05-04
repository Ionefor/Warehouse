using Warehourse.ProductPlacement.Application.Features.Query.Product.GetByCategory;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Presentation.Requests;

public record GetProductsWithPaginationRequest(
    PaginationParamsDto? PaginationParams, string Category)
{
    public GetProductsWithPaginationQuery ToQuery() => new(PaginationParams, Category);
}