using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Infrastructure.BackgroundServices;
using Warehourse.ProductPlacement.Infrastructure.DbContexts;
using Warehourse.ProductPlacement.Infrastructure.Repositories;
using Warehourse.ProductPlacement.Infrastructure.Services;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models;

namespace Warehourse.ProductPlacement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductPlacementInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContexts(configuration).
            AddRepositories().
            AddDatabase().AddServices();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    { 
        services.AddScoped<IProductPlacementRepository, ProductPlacementRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProductPlacementService, ProductPlacementService>();
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        services.AddHostedService<ProductPlacementBackgroundService>();
        return services;
    }
    
    private static IServiceCollection AddDbContexts(
        this IServiceCollection services, IConfiguration configuration)
    {
       services.AddDbContext<ProductWriteDbContext>();
        services.AddDbContext<IReadDbContext, ProductReadDbContext>();

        return services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModulesName.ProductPlacement);
        
        return services;
    }
}