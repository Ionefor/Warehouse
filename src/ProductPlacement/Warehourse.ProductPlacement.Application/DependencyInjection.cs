using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Warehouse.Core.Abstractions;
using Warehouse.SharedKernel.Models;

namespace Warehourse.ProductPlacement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProductPlacementApplication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.
            AddCommands().
            AddServices(configuration).
            AddQueries().
            AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        return services;
    }
    
    private static IServiceCollection AddServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(provider => 
        {
            var redisConnectionString = configuration.
                GetConnectionString(Constants.Shared.Redis);
            
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        services.AddScoped<IDatabase>(provider => 
        {
            var redis = provider.GetRequiredService<IConnectionMultiplexer>();
            return redis.GetDatabase();
        });
        
        return services;
    }
    
    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly).
            AddClasses(classes => classes.
                AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>))).
            AsSelfWithInterfaces().
            WithScopedLifetime());
        
        return services;
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly).
            AddClasses(classes => classes.
                AssignableTo(typeof(IQueryHandler<,>))).
            AsSelfWithInterfaces().
            WithScopedLifetime());
        
        return services;
    }
}