using Warehouse.Core.Abstractions;
using Warehouse.Core.Extensions;
using Warehouse.Core.Models;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Features.Query.Section.GetByWarehouseId;

public class GetByWarehouseIdHandler :
    IQueryHandler<PageList<FullSectionDto>, GetSectionsWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetByWarehouseIdHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PageList<FullSectionDto>> Handle(
        GetSectionsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var sections = _context.Sections;
        
        return await sections.ToPagedList(
            query.PaginationParams!.Page,
            query.PaginationParams.PageSize, cancellationToken);
    }
}