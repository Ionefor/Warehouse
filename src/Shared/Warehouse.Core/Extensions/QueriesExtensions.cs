using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Models;

namespace Warehouse.Core.Extensions;

public static class QueriesExtensions
{
    public static async Task<PageList<T>> ToPagedList<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        
        var items = await source.
            Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PageList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
    
    public static PageList<T> ToPagedList<T>(
        this IEnumerable<T> source,
        int page,
        int pageSize)
    {
        var totalCount = source.Count();
        
        var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PageList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
    
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
    
    public static IQueryable<TSource> SortIf<TSource, TKey>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, TKey>> selector)
    {
        return condition ? source.OrderBy(selector) : source;
    }
}