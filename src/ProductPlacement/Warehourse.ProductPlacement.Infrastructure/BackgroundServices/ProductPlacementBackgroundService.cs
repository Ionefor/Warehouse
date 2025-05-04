using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Warehourse.ProductPlacement.Application.Abstractions;

namespace Warehourse.ProductPlacement.Infrastructure.BackgroundServices;

public class ProductPlacementBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ProductPlacementBackgroundService> _logger;

    public ProductPlacementBackgroundService(
        ILogger<ProductPlacementBackgroundService> logger,
        IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Сервис распределения товаров запущен");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await WaitForNextSchedule(cancellationToken);
                await ProcessPlacement(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в сервисе распределения товаров");
                
                await Task.Delay(TimeSpan.FromHours(1), cancellationToken); 
            }
        }
    }
    
    private async Task ProcessPlacement(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        
        var placementService = scope.ServiceProvider.
            GetRequiredService<IProductPlacementService>();
        
        await placementService.PendingProductPlacement(cancellationToken);
    }
    
    private async Task WaitForNextSchedule(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        
        var nextRun = new DateTime(
            now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        
        if (now > nextRun)
        {
            nextRun = nextRun.AddMinutes(1);
        }
    
        var delay = nextRun - now;
        
        if (delay.TotalMilliseconds > int.MaxValue)
        {
            delay = TimeSpan.FromMilliseconds(int.MaxValue - 1);
        }
        else if (delay.TotalMilliseconds < 0)
        {
            delay = TimeSpan.Zero;
        }
    
        _logger.LogInformation($"Следующее выполнение запланировано на" +
                               $" {nextRun} (через {delay.TotalMinutes} минут)");
    
        await Task.Delay(delay, cancellationToken);
    }
}