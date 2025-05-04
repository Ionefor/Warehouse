using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehouse.SharedKernel.ValueObjects.Ids;

namespace Warehourse.ProductPlacement.Application.Abstractions;

public interface IProductPlacementRepository
{
    Task AddProduct(Product product, CancellationToken cancellationToken = default);
    
    Task AddPendingProduct(PendingProduct product, CancellationToken cancellationToken = default);
    
    Task AddStorage(ProductStorage storage, CancellationToken cancellationToken = default);

    void Delete(PendingProduct product);
    
    void Delete(ProductStorage storage);
    
    void Delete(Product product);
    
    Task<ProductStorage> GetStorage(ProductStorageId productStorageId, CancellationToken cancellationToken = default);

    Task<Product> GetProduct(ProductId productId, CancellationToken cancellationToken = default);
    
    List<PendingProduct> GetAllPendingProducts();

    Task<List<ProductStorage>> GetProductStorages(
        Guid warehouseId,
        CancellationToken cancellationToken = default);
}