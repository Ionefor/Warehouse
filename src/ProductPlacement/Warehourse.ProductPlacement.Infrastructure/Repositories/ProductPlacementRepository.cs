using Microsoft.EntityFrameworkCore;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehourse.ProductPlacement.Infrastructure.DbContexts;
using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehourse.ProductPlacement.Infrastructure.Repositories;

public class ProductPlacementRepository(ProductWriteDbContext context)
    : IProductPlacementRepository
{
    public async Task AddProduct(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await context.Products.AddAsync(product, cancellationToken);
    }

    public async Task AddPendingProduct(
        PendingProduct product,
        CancellationToken cancellationToken = default)
    {
        await context.PendingProducts.AddAsync(product, cancellationToken);
    }

    public async Task AddStorage(
        ProductStorage storage,
        CancellationToken cancellationToken = default)
    {
        await context.Storages.AddAsync(storage, cancellationToken);
    }

    public void Delete(PendingProduct product)
    {
        context.PendingProducts.Remove(product!);
    }

    public void Delete(ProductStorage storage)
    {
        context.Storages.Remove(storage);
    }

    public void Delete(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<ProductStorage> GetStorage(
        ProductStorageId productStorageId,
        CancellationToken cancellationToken = default)
    {
        return (await context.Storages.
            FirstOrDefaultAsync(
                s => s.Id == productStorageId, cancellationToken))!;
    }

    public async Task<Product> GetProduct(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        return (await context.Products.
            FirstOrDefaultAsync(
                s => s.Id == productId, cancellationToken))!;
    }

    public List<PendingProduct> GetAllPendingProducts()
    {
        return context.PendingProducts.ToList();
    }

    public async Task<List<ProductStorage>> GetProductStorages(
        Guid warehouseId,
        CancellationToken cancellationToken = default)
    {
       return await context.Storages.
            Where(s => s.WarehouseId == warehouseId).
            ToListAsync(cancellationToken);
    }
}