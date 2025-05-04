using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Warehouse.Core.Abstractions;
using Warehouse.WarehouseManagement.Infrastructure.DbContexts;

namespace Warehouse.WarehouseManagement.Infrastructure;

public class UnitOfWork(WarehouseWriteDbContext dbContext) : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}