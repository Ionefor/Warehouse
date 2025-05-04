using System.Text.Json;
using CSharpFunctionalExtensions;
using StackExchange.Redis;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Contracts.Dtos;
using Warehouse.Core.Abstractions;
using Warehouse.Core.Extensions;
using Warehouse.Core.Models;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehourse.ProductPlacement.Application.Features.Query.ProductStorage.GetAll;

public class GetProductStoragesWithPaginationHandler : 
    IQueryHandler<PageList<ProductStorageDto>, GetProductStoragesWithPaginationQuery>
{
    private readonly IReadDbContext _context;
    private readonly IDatabase _redis;
    
    public GetProductStoragesWithPaginationHandler(
        IReadDbContext context, IDatabase redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<PageList<ProductStorageDto>> Handle(
        GetProductStoragesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        string cacheKey =
            $"pending_products:{query.PaginationParams.Page}" +
            $":{query.PaginationParams.PageSize}";
        
        var cachedData = await _redis.StringGetAsync(cacheKey);
        
        if (cachedData.HasValue)
        {
            return JsonSerializer.Deserialize<PageList<ProductStorageDto>>(cachedData);
        }
        
        var productStorages = _context.ProductStorages;
        
        var result = await productStorages.ToPagedList(
            query.PaginationParams!.Page,
            query.PaginationParams.PageSize, cancellationToken);
        
        await _redis.StringSetAsync(
            cacheKey,
            JsonSerializer.Serialize(result),
            TimeSpan.FromMinutes(5));

        return result;
    }
}