using Microsoft.Extensions.DependencyInjection;
using Warehourse.ProductPlacement.Contracts;

namespace Warehourse.ProductPlacement.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddProductPlacementPresentation(this IServiceCollection services)
    {
        return services.AddScoped<IProductPlacementContract, ProductPlacementContract>();
        return services;
    }
}