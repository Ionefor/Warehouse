using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models;
using Warehouse.SharedKernel.Models.Errors;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Application.Features.Query.View.GetWarehouseView;

public class GetWarehouseViewHandler
    : IQueryHandler<Result<WarehouseViewDto, ErrorList>, GetWarehouseViewQuery>
{
    private readonly IReadDbContext _context;

    public GetWarehouseViewHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<Result<WarehouseViewDto, ErrorList>> Handle(
        GetWarehouseViewQuery query,
        CancellationToken cancellationToken = default)
    {
        var warehouse = await _context.Warehouses.
            FirstOrDefaultAsync(w => w.Id == query.WarehouseId, cancellationToken);

        if (warehouse is null)
        {
            return Errors.General.NotFound(
                new ErrorParameters.NotFound(nameof(Warehouse),
                    nameof(query.WarehouseId), query.WarehouseId)).ToErrorList();
        }
        
        return new WarehouseViewDto
        {
            SectionCount = warehouse.Sections.Length,
            LargeSectionCount = warehouse.Sections.
                Select(s => s.Type is SectionType.Large).Count(),
            MediumSectionCount = warehouse.Sections.
                Select(s => s.Type is SectionType.Medium).Count(),
            SmallSectionCount = warehouse.Sections.
                Select(s => s.Type is SectionType.Small).Count(),
            Name = warehouse.Name,
            WarehouseFullness = GetFullness(warehouse.Sections)
        };
    }

    private string GetFullness(FullSectionDto[] sections)
    {
        int shelfsCount = 0;
        int shelfsUnAvailableCount = 0;
        
        foreach (var section in sections)
        {
            var currentShelfsCount = 
                section.SectionRows.Count * 
                section.SectionRows.First().Shelfs.Count();
            
            var currentShelfsUnAvailableCount = section.SectionRows
                .Sum(row => row.Shelfs.Count(shelf => !shelf.IsAvailable)); 
            
            shelfsCount += currentShelfsCount;
            shelfsUnAvailableCount += currentShelfsUnAvailableCount;
        }
        
        var warehouseFullness = shelfsUnAvailableCount / shelfsCount!;

        var roundedFullness = Math.Round((double)warehouseFullness, 1);
        
        return roundedFullness * 100 + "%";
    }
}