using System.Text.Json;
using CSharpFunctionalExtensions;
using StackExchange.Redis;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Contracts.Dtos;
using Warehouse.Core.Abstractions;
using Warehouse.Core.Extensions;
using Warehouse.Core.Models;
using Warehouse.SharedKernel.Models.Errors;

namespace Warehourse.ProductPlacement.Application.Features.Query.PendingProduct.GetAll;

public class GetPendingProductsWithPaginationHandler
    : IQueryHandler<PageList<PendingProductDto>, GetPendingProductsWithPaginationQuery>
{
    private readonly IReadDbContext _context;
    private readonly IDatabase _redis;
    
    public GetPendingProductsWithPaginationHandler(
        IReadDbContext context,
        IDatabase redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<PageList<PendingProductDto>> Handle(
        GetPendingProductsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        string cacheKey =
            $"pending_products:{query.PaginationParams.Page}" +
            $":{query.PaginationParams.PageSize}";
        
        var cachedData = await _redis.StringGetAsync(cacheKey);
        
        if (cachedData.HasValue)
        {
            return JsonSerializer.Deserialize<PageList<PendingProductDto>>(cachedData);
        }
        
        var pendingProducts = _context.PendingProducts;
        
        var result = await pendingProducts.ToPagedList(
            query.PaginationParams!.Page,
            query.PaginationParams.PageSize, cancellationToken);
        
        await _redis.StringSetAsync(
            cacheKey,
            JsonSerializer.Serialize(result),
            TimeSpan.FromMinutes(5));

        return result;
    }
}