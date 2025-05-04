using Microsoft.Extensions.DependencyInjection;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models;
using Warehouse.WarehouseManagement.Application.Abstractions;
using Warehouse.WarehouseManagement.Infrastructure.DbContexts;
using Warehouse.WarehouseManagement.Infrastructure.Repositories;
using Warehouse.WarehouseManagement.Infrastructure.Services;

namespace Warehouse.WarehouseManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWarehouseInfrastructure(this IServiceCollection services)
    {
        services.AddDbContexts().
            AddRepositories().
            AddDatabase().AddServices();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    { 
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPackerService, PackerService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        
        return services;
    }
    
    private static IServiceCollection AddDbContexts(
        this IServiceCollection services)
    {
        services.AddDbContext<WarehouseWriteDbContext>();
        services.AddDbContext<IReadDbContext, WarehouseReadDbContext>();

        return services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<
            IUnitOfWork, UnitOfWork>(ModulesName.WarehouseManagement);
        
        return services;
    }
}