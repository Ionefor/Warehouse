using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Features.Query.View.GetSectionView;

public class GetSectionViewHandler
    : IQueryHandler<Result<SectionViewDto, ErrorList>, GetSectionViewQuery>
{
    private readonly IReadDbContext _context;

    public GetSectionViewHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SectionViewDto, ErrorList>> Handle(
        GetSectionViewQuery query,
        CancellationToken cancellationToken = default)
    {
        var section = await _context.Sections.
            FirstOrDefaultAsync(
                w => w.Id == query.SectionId &&
                     w.WarehouseId == query.WarehouseId,
                        cancellationToken);

        if (section is null)
        {
            return Errors.General.NotFound(
                new ErrorParameters.NotFound(nameof(Warehouse),
                    nameof(query.SectionId), query.SectionId)).ToErrorList();
        }

        return new SectionViewDto
        {
            SectionRowCount = section.SectionRows.Count,
            SectionRowShelfsCount = section.
                SectionRows.First().Shelfs.Count(),
            Type = section.Type,
            SectionFullness = GetFullness(section)
        };
    }
    
    private string GetFullness(FullSectionDto section)
    {
        var shelfsCount = 
            section.SectionRows.Count * 
            section.SectionRows.First().Shelfs.Count();
            
        var shelfsUnAvailableCount = section.SectionRows
            .Sum(row => row.Shelfs.Count(shelf => !shelf.IsAvailable)); 
        
        var warehouseFullness = shelfsUnAvailableCount / shelfsCount!;

        var roundedFullness = Math.Round((double)warehouseFullness, 1);
        
        return roundedFullness * 100 + "%";
    }
}