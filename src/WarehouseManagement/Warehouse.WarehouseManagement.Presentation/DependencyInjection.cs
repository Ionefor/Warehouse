using Microsoft.Extensions.DependencyInjection;
using Warehouse.WarehouseManagement.Contracts;

namespace Warehouse.WarehouseManagement.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddWarehousePresentation(this IServiceCollection services)
    {
        services.AddScoped<IWarehouseManagementContract, WarehouseManagementContract>();
        
        return services;
    }
}