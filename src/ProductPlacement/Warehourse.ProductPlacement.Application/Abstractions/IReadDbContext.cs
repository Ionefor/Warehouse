using Warehourse.ProductPlacement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Application.Abstractions;

public interface IReadDbContext
{
    IQueryable<ProductStorageDto> ProductStorages { get; }
    
    IQueryable<ProductDto> Products { get; }
    
    IQueryable<PendingProductDto> PendingProducts { get; }
}