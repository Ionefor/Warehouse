using Warehouse.Core.Abstractions;
using Warehouse.Core.Extensions;
using Warehouse.Core.Models;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Features.Query.Warehouse.GetAll;

public class GetWarehousesWithPaginationHandler :
    IQueryHandler<PageList<WarehouseDto>, GetWarehousesWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetWarehousesWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PageList<WarehouseDto>> Handle(
        GetWarehousesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var warehouses = _context.Warehouses;
        
        return await warehouses.ToPagedList(
            query.PaginationParams!.Page,
            query.PaginationParams.PageSize, cancellationToken);
    }
}