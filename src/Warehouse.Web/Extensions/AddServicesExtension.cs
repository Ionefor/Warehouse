using FluentValidation;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Warehourse.ProductPlacement.Application;
using Warehourse.ProductPlacement.Infrastructure;
using Warehourse.ProductPlacement.Presentation;
using Warehouse.WarehouseManagement.Application;
using Warehouse.WarehouseManagement.Infrastructure;
using Warehouse.WarehouseManagement.Presentation;

namespace Warehouse.Web.Extensions;

public static class AddServicesExtension
{
     public static IServiceCollection AddServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.
            AddWarehouseModule().
            AddProductPlacementModule(configuration).
            AddCustomSwaggerGen().
            AddValidation(configuration).
            AddLogging(configuration);

        return services;
    }
    
    private static IServiceCollection AddValidation(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        return services;
    }
    
    private static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Banking API",
                Version = "1"
            });
        });

        return services;
    }
    
    private static IServiceCollection AddLogging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(configuration.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq"))
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

        services.AddSerilog();
        services.AddHttpLogging(o => { o.CombineLogs = true; });

        return services;
    }
    
    private static IServiceCollection AddWarehouseModule(
        this IServiceCollection services)
    {
        services.
            AddWarehouseInfrastructure().
            AddWarehouseApplication().
            AddWarehousePresentation();

        return services;
    }

    private static IServiceCollection AddProductPlacementModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.
            AddProductPlacementInfrastructure(configuration).
            AddProductPlacementApplication(configuration).
            AddProductPlacementPresentation();

        return services;
    }
    
}