using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Warehourse.ProductPlacement.Infrastructure.DbContexts;
using Warehouse.Core.Abstractions;

namespace Warehourse.ProductPlacement.Infrastructure;

public class UnitOfWork(ProductWriteDbContext dbContext) : IUnitOfWork
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