using Microsoft.EntityFrameworkCore;
using Warehourse.ProductPlacement.Contracts.Dtos;
using Warehouse.Core.Dtos;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Contracts;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehouse.WarehouseManagement.Presentation;

public class WarehouseManagementContract : IWarehouseManagementContract
{
    private readonly IWarehouseService _service;
    private readonly IWarehouseRepository _repository;
    private readonly IReadDbContext _readDbContext;

    public WarehouseManagementContract(
        IWarehouseService service,
        IWarehouseRepository repository,
        IReadDbContext readDbContext)
    {
        _service = service;
        _repository = repository;
        _readDbContext = readDbContext;
    }

    public async Task<ProductStorageDto?> GetStorage(
        PendingProductDto product,
        IEnumerable<Guid> warehouseIds,
        CancellationToken cancellationToken = default)
    {
        _service.AddWarehouse(warehouseIds);
        
        return await _service.FindPlacement(product, cancellationToken);
    }

    public Task<List<Guid>> GetAllWarehouseIds(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllWarehouseIds(cancellationToken);
    }

    public async Task<WarehouseDto> GetWarehouse(
        Guid warehouseId,
        CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.
            GetWarehouse(warehouseId, cancellationToken);

        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name.Value,
            Email = warehouse.NotificationEmail.Value,
            Size = new SizeDto(
                warehouse.Size.Length,
                warehouse.Size.Width,
                warehouse.Size.Height),
            Sections = warehouse.
                Sections.
                Select(s => new FullSectionDto
                {
                    Id = s.Id,
                    WarehouseId = warehouse.Id,
                    Type = s.Type,
                    Size = new SizeDto(s.Size.Length, s.Size.Width, s.Size.Height),
                    SectionRows = s.Rows.
                        Values.Select(r => new SectionRowDto(
                            new SizeDto(r.Size.Length, r.Size.Width, r.Size.Height),
                            r.Number,
                            r.Shelfs.Values.Select(
                                sh => new ShelfDto(
                                    sh.IsAvailable,
                                    new SizeDto(r.Size.Length,
                                        r.Size.Width,
                                        r.Size.Height), sh.Row, sh.Column)))).ToList()
                }).ToArray()
        };
    }

    public async Task<bool> AvailableShelf(
        Guid warehouseId, Guid sectionId,
        int rowNumber, int shelfRow, int shelfColumn,
        SizeDto productSize, CancellationToken cancellationToken)
    {
       var warehouse = await _readDbContext.
           Warehouses.FirstOrDefaultAsync(
               w => w.Id == warehouseId, cancellationToken);

       if (warehouse is null)
           return false;
       
       var section = warehouse.Sections.
           FirstOrDefault(s => s.Id == sectionId);
       
       if(section is null)
           return  false;

       var availableShelf = section.SectionRows.FirstOrDefault(
           r => r.Number == rowNumber &&
                r.Shelfs.FirstOrDefault(
                    s => s.IsAvailable &&
                         s.Column == shelfColumn &&
                         s.Row == shelfRow &&
                         s.Size.Height >= productSize.Height &&
                         s.Size.Width >= productSize.Width &&
                         s.Size.Length >= productSize.Length) is not null);
       
       if(availableShelf is null)
           return false;

       return true;
    }
}