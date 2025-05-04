using Microsoft.EntityFrameworkCore;
using Warehouse.SharedKernel.ValueObjects.Ids;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Infrastructure.DbContexts;

namespace Warehouse.WarehouseManagement.Infrastructure.Repositories;

public class WarehouseRepository(WarehouseWriteDbContext dbContext) : IWarehouseRepository
{
    public async Task Add(Domain.Aggregate.Warehouse warehouse, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(warehouse, cancellationToken);
    }

    public void Delete(Domain.Aggregate.Warehouse warehouse)
    {
        dbContext.Remove(warehouse!);
    }

    public async Task<Domain.Aggregate.Warehouse> GetWarehouse(
        WarehouseId warehouseId, CancellationToken cancellationToken)
    {
       return (await dbContext.Warehouses.Include(w => w.Sections).
           FirstOrDefaultAsync(w => w.Id == warehouseId, cancellationToken))!;
    }

    public async Task<List<Guid>> GetAllWarehouseIds(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.
            Warehouses.
            Select(w => w.Id.Id).
            ToListAsync(cancellationToken);
    }
}